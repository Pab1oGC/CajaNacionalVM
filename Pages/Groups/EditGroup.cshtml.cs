using CNSVM.Data;
using CNSVM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CNSVM.Pages.Groups
{
    public class EditGroupModel : PageModel
    {
        private readonly CnsvmDbContext _cnsvmDbContext;
        public EditGroupModel(CnsvmDbContext db)
        {
            _cnsvmDbContext = db;
        }

        [BindProperty]
        public MedicalGroup Group { get; set; }
        public IEnumerable<User> Doctors { get; set; }
        public IEnumerable<DoctorGroup> ExistingDoctorGroups { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Group = await _cnsvmDbContext.MedicalGroup.FindAsync(id);
            if (Group == null)
            {
                return NotFound();
            }

            //ExistingDoctorGroups = await _cnsvmDbContext.DoctorGroup.Where(dg => dg.GroupId == id).ToListAsync();
            //Doctors = await _cnsvmDbContext.User
            //    .Join(
            //    _cnsvmDbContext.MedicalGroup,
            //    doctor =>doctor.Id,
            //    medicalG=>medicalG.UserId,
            //    (doctor,medicalG)=> new { 
            //        Doctor = doctor,
            //        MedicalG=medicalG
            //    }
            //    )
            //    .OrderBy(doctor => doctor.Name).ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, string ids)
        {
            if (ids == null)
            {
                ModelState.AddModelError("Group", "No puedes editar el grupo sin integrantes");
                Doctors = await _cnsvmDbContext.User.OrderBy(doctor => doctor.Name).ToListAsync();
                return Page();
            }

            if (!ModelState.IsValid)
            {
                Doctors = await _cnsvmDbContext.User.OrderBy(doctor => doctor.Name).ToListAsync();
                return Page();
            }

            try
            {
                var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                int doctorId = int.Parse(doctorIdClaim.Value);
                Group.CreatedBy = doctorId;
                Group.CreatedAt = DateTime.Now;

                _cnsvmDbContext.Attach(Group).State = EntityState.Modified;
                await _cnsvmDbContext.SaveChangesAsync();

                var currentDoctorGroupIds = ExistingDoctorGroups.Select(dg => dg.UserId).ToList();

                // Eliminar los doctores que ya no están en el grupo
                var idsToAdd = ids.Split(',').Select(int.Parse).ToList();
                var idsToRemove = currentDoctorGroupIds.Except(idsToAdd).ToList();

                foreach (var idToRemove in idsToRemove)
                {
                    var doctorGroup = await _cnsvmDbContext.DoctorGroup
                        .FirstOrDefaultAsync(dg => dg.GroupId == id && dg.UserId == idToRemove);
                    if (doctorGroup != null)
                    {
                        _cnsvmDbContext.DoctorGroup.Remove(doctorGroup);
                    }
                }

                await _cnsvmDbContext.SaveChangesAsync();

                // Añadir nuevos doctores al grupo
                foreach (var idd in idsToAdd)
                {
                    if (!currentDoctorGroupIds.Contains(id))
                    {
                        await _cnsvmDbContext.DoctorGroup.AddAsync(new DoctorGroup()
                        {
                            GroupId = id,
                            UserId = id
                        });
                    }
                }

                await _cnsvmDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToPage("Index"); // Redirige a la página de lista de grupos
        }
    }
}