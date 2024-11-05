using CNSVM.Data;
using CNSVM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace CNSVM.Pages.Groups
{
    public class EditModel : PageModel
    {
        private readonly CnsvmDbContext _cnsvmDbContext;
        private const int InitialLoadLimit = 5;

        public EditModel(CnsvmDbContext db)
        {
            _cnsvmDbContext = db;
        }

        [BindProperty]
        public MedicalGroup Group { get; set; }
        public IEnumerable<User> Doctors { get; set; }
        public IEnumerable<User> Members { get; set; }
        [BindProperty]
        public string Ids { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Group = await _cnsvmDbContext.MedicalGroup.FindAsync(id);

            if (Group == null)
            {
                return NotFound();
            }

            Doctors = await _cnsvmDbContext.User
                .Where(d => !_cnsvmDbContext.DoctorGroup.Any(m => m.GroupId == id && m.UserId == d.Id))
                .OrderBy(d => d.Name)
                .Take(InitialLoadLimit)
                .ToListAsync();

            Members = await _cnsvmDbContext.User
                .Where(u => _cnsvmDbContext.DoctorGroup.Any(m => m.GroupId == id && m.UserId == u.Id))
                .OrderBy(u => u.Name)
                .ToListAsync();

            Ids = string.Join(",", Members.Select(m => m.Id));

            return Page();
        }

        public async Task<IActionResult> OnPostSaveAsync(string Ids)
        {
            if (string.IsNullOrWhiteSpace(Ids))
            {
                ModelState.AddModelError("Group", "El grupo debe tener al menos un miembro.");
                await LoadPageDataAsync(Group.Id); // Cargar datos en caso de error
                return Page();
            }

            if (!ModelState.IsValid)
            {
                await LoadPageDataAsync(Group.Id); // Cargar datos en caso de error
                return Page();
            }

            await UpdateGroupMembersAsync(Ids);
            return RedirectToPage("Index");
        }

        // Método auxiliar para cargar datos de la página
        private async Task LoadPageDataAsync(int id)
        {
            Group = await _cnsvmDbContext.MedicalGroup.FindAsync(id);
            Doctors = await _cnsvmDbContext.User
                .Where(d => !_cnsvmDbContext.DoctorGroup.Any(m => m.GroupId == id && m.UserId == d.Id))
                .OrderBy(d => d.Name)
                .Take(InitialLoadLimit)
                .ToListAsync();

            Members = await _cnsvmDbContext.User
                .Where(u => _cnsvmDbContext.DoctorGroup.Any(m => m.GroupId == id && m.UserId == u.Id))
                .OrderBy(u => u.Name)
                .ToListAsync();
        }



        private async Task UpdateGroupMembersAsync(string ids)
        {
            var doctorIds = ids.Split(',').Select(int.Parse).ToList();

            var existingMembers = _cnsvmDbContext.DoctorGroup.Where(m => m.GroupId == Group.Id);
            _cnsvmDbContext.DoctorGroup.RemoveRange(existingMembers);

            foreach (var id in doctorIds)
            {
                _cnsvmDbContext.DoctorGroup.Add(new DoctorGroup
                {
                    GroupId = Group.Id,
                    UserId = id
                });
            }

            await _cnsvmDbContext.SaveChangesAsync();
        }
    }
}
