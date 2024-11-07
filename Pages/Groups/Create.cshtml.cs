using CNSVM.Data;
using CNSVM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace CNSVM.Pages.Groups
{
    
    public class CreateModel : PageModel
    {
        private readonly CnsvmDbContext _cnsvmDbContext;
        private const int InitialLoadLimit = 5;

        public CreateModel(CnsvmDbContext db)
        {
            _cnsvmDbContext = db;
        }

        [BindProperty]
        public MedicalGroup Group { get; set; }
        public IEnumerable<User> Doctors { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Cargar solo los primeros 20 doctores
            Doctors = await _cnsvmDbContext.User
                .OrderBy(doctor => doctor.Name)
                .Take(InitialLoadLimit)
                .ToListAsync();
            return Page();
        }

        // Método para la búsqueda de doctores
        public async Task<JsonResult> OnGetSearchDoctorsAsync(string query)
        {
            var doctors = await _cnsvmDbContext.User
                .Where(d => d.Name.Contains(query) || d.FirstName.Contains(query) || d.LastName.Contains(query))
                .OrderBy(d => d.Name)
                .Select(d => new
                {
                    id = d.Id,
                    name = d.Name,
                    firstName = d.FirstName,
                    lastName = d.LastName
                }).Take(InitialLoadLimit)
                .ToListAsync();

            return new JsonResult(doctors);
        }
        public async Task<JsonResult> OnGetInitialDoctorsAsync()
        {
            var doctors = await _cnsvmDbContext.User
                .OrderBy(d => d.Name)
                .Take(5)
                .Select(d => new
                {
                    id = d.Id,
                    name = d.Name,
                    firstName = d.FirstName,
                    lastName = d.LastName
                })
                .ToListAsync();

            return new JsonResult(doctors);
        }


        public async Task<IActionResult> OnPost(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                ModelState.AddModelError("Group", "No puedes crear un grupo sin integrantes.");
                Doctors = await _cnsvmDbContext.User
                    .OrderBy(d => d.Name)
                    .Take(InitialLoadLimit)
                    .ToListAsync();
                return Page();
            }

            if (!ModelState.IsValid)
            {
                Doctors = await _cnsvmDbContext.User
                    .OrderBy(d => d.Name)
                    .Take(InitialLoadLimit)
                    .ToListAsync();
                return Page();
            }

            try
				Doctors = await _cnsvmDbContext.User.OrderBy(doctor => doctor.Name).ToListAsync();
				return Page();
            }
			bool existName = await _cnsvmDbContext.MedicalGroup.AnyAsync(x => x.Name == Group.Name);
            if (existName)
            {
				ModelState.AddModelError("NameExist", "El nombre del grupo ya existe");
				Doctors = await _cnsvmDbContext.User.OrderBy(doctor => doctor.Name).ToListAsync();
				return Page();
			}
			try
            {
                await CreateGroupAsync(ids);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear el grupo. Inténtalo de nuevo.");
                return Page();
            }

            return RedirectToPage("Index");
        }

        private async Task CreateGroupAsync(string ids)
        {
            var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int doctorId = int.Parse(doctorIdClaim.Value);

            Group.CreatedBy = doctorId;
            Group.CreatedAt = DateTime.Now;
            Group.Status = "A";

            await _cnsvmDbContext.MedicalGroup.AddAsync(Group);
            await _cnsvmDbContext.SaveChangesAsync();

            int groupId = Group.Id;
            string[] doctorIds = ids.Split(',');

            foreach (var id in doctorIds)
            {
                await _cnsvmDbContext.DoctorGroup.AddAsync(new DoctorGroup()
                {
                    GroupId = groupId,
                    UserId = int.Parse(id)
                });
            }

                await _cnsvmDbContext.SaveChangesAsync();
				return RedirectToPage(new { showModal = true });
			}
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
