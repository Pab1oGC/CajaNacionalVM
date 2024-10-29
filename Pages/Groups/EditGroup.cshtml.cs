using CNSVM.Data;
using CNSVM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CNSVM.Pages.Groups
{
    public class EditGroupModel : PageModel
    {
        private readonly CnsvmDbContext _context;

        public EditGroupModel(CnsvmDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int GroupId { get; set; }

        [BindProperty]
        public string GroupName { get; set; }

        [BindProperty]
        public string GroupStatus { get; set; }

        public List<User> GroupDoctors { get; set; } = new List<User>();
        public List<User> AvailableDoctors { get; set; } = new List<User>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Cargar el grupo y los doctores
            MedicalGroup group = await _context.MedicalGroup
                .Include(g => g.DoctorGroups)
                    .ThenInclude(dg => dg.User)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            GroupId = group.Id;
            GroupName = group.Name;
            GroupStatus = group.Status;

            GroupDoctors = group.DoctorGroups.Select(dg => dg.User).ToList();
            // Extraer los IDs de los doctores en el grupo
            var groupDoctorIds = GroupDoctors.Select(gd => gd.Id).ToList();

            // Consultar los usuarios que no están en el grupo
            AvailableDoctors = await _context.User
                .Where(u => !groupDoctorIds.Contains(u.Id))
                .ToListAsync();


            return Page();
        }

        public async Task<IActionResult> OnPostUpdateGroupAsync(int groupId, string groupName, string groupStatus, List<int> doctorIds)
        {
            var group = await _context.MedicalGroup
                .Include(g => g.DoctorGroups)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null)
            {
                return NotFound();
            }

            // Actualizar nombre y estado del grupo
            group.Name = groupName;
            group.Status = groupStatus;

            // Actualizar miembros del grupo
            var currentDoctorIds = group.DoctorGroups.Select(dg => dg.UserId).ToList();

            // Doctores a eliminar
            var doctorsToRemove = currentDoctorIds.Except(doctorIds).ToList();
            foreach (var doctorId in doctorsToRemove)
            {
                var doctorGroup = group.DoctorGroups.FirstOrDefault(dg => dg.UserId == doctorId);
                if (doctorGroup != null)
                {
                    _context.DoctorGroup.Remove(doctorGroup);
                }
            }

            // Doctores a agregar
            var doctorsToAdd = doctorIds.Except(currentDoctorIds).ToList();
            foreach (var doctorId in doctorsToAdd)
            {
                group.DoctorGroups.Add(new DoctorGroup
                {
                    GroupId = groupId,
                    UserId = doctorId
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("/Groups");
        }
    }
}
