using Microsoft.AspNetCore.Authorization;
using CNSVM.Data;
using CNSVM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CNSVM.Pages.Patients
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly CnsvmDbContext _cnsvmDbContext;

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }
        public IndexModel(CnsvmDbContext cnsvmDbContext)
        {
            _cnsvmDbContext = cnsvmDbContext;
        }

        public IEnumerable<Patient> Patients { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var query = _cnsvmDbContext.Patient.AsQueryable();

            // If there's a search query, filter results and ignore case
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                query = query.Where(p => EF.Functions.Like(p.Name.ToLower(), $"%{SearchQuery.ToLower()}%"));
            }

            Patients = await query.ToListAsync();
            return Page();
        }
    }
}
