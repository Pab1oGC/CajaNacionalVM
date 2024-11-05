using CNSVM.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CNSVM.Models;

namespace CNSVM.Pages.Groups
{
    public class DetailsModel : PageModel
    {
        private readonly CnsvmDbContext _context;

      

        public DetailsModel(CnsvmDbContext context)
        {
            _context = context;
        }
        public string GroupName { get; set; }
        public List<User> GroupMembers { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var group = await _context.MedicalGroup
                .Include(g => g.DoctorGroups)
                    .ThenInclude(dg => dg.User)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            GroupName = group.Name;
            GroupMembers = group.DoctorGroups
                .Select(dg => dg.User)
                .ToList();

            return Page();
        }
    }
}
