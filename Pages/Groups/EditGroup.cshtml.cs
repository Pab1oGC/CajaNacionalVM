using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CNSVM.Pages.Groups
{
    public class EditGroupModel : PageModel
    {
        //http://localhost:5082/Groups/EditGroup?id=1
        public int GroupId { get; set; }
        public List<Doctor> GroupDoctors { get; set; } = new List<Doctor>();
        public List<Doctor> AvailableDoctors { get; set; } = new List<Doctor>();
        [BindProperty]
        public string SearchQuery { get; set; }

        public async Task OnGetAsync(int groupId)
        {
            GroupId = groupId;
            GroupDoctors = await GetGroupDoctorsAsync(groupId);
            AvailableDoctors = await GetAvailableDoctorsAsync(SearchQuery);
        }

        public async Task<IActionResult> OnPostAddDoctorAsync(int doctorId, int groupId)
        {
            await AddDoctorToGroupAsync(doctorId, groupId);
            return RedirectToPage(new { GroupId = groupId });
        }

        public async Task<IActionResult> OnPostRemoveDoctorAsync(int doctorId, int groupId)
        {
            await RemoveDoctorFromGroupAsync(doctorId, groupId);
            return RedirectToPage(new { GroupId = groupId });
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {
            AvailableDoctors = await GetAvailableDoctorsAsync(SearchQuery);
            return Page();
        }

        private Task<List<Doctor>> GetGroupDoctorsAsync(int groupId)
        {
            // Simula la obtención de médicos que ya están en el grupo desde la base de datos.
            return Task.FromResult(new List<Doctor>
            {
                new Doctor { Id = 1, Name = "Dr. Juan Perez" },
                new Doctor { Id = 2, Name = "Dr. Maria Lopez" }
            });
        }

        private Task<List<Doctor>> GetAvailableDoctorsAsync(string searchQuery)
        {
            // Simula la obtención de médicos disponibles (excluye los del grupo actual) desde la base de datos.
            var allDoctors = new List<Doctor>
            {
                new Doctor { Id = 3, Name = "Dr. Carlos Diaz" },
                new Doctor { Id = 4, Name = "Dr. Ana Ramirez" },
                new Doctor { Id = 5, Name = "Dr. Luis Rodriguez" }
            };

            return Task.FromResult(allDoctors
                .Where(d => string.IsNullOrEmpty(searchQuery) || d.Name.Contains(searchQuery))
                .ToList());
        }

        private Task AddDoctorToGroupAsync(int doctorId, int groupId)
        {
            // Añadir médico a un grupo en la base de datos.
            return Task.CompletedTask;
        }

        private Task RemoveDoctorFromGroupAsync(int doctorId, int groupId)
        {
            // Quitar médico de un grupo en la base de datos.
            return Task.CompletedTask;
        }
    }

    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
