using CNSVM.Data;
using CNSVM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CNSVM.Pages.Doctors
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

        public IEnumerable<User> Doctors { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var query = _cnsvmDbContext.User.AsQueryable();

            // Si hay una consulta de búsqueda, filtrar los resultados y no considerar mayúsculas/minúsculas
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                query = query.Where(p => EF.Functions.Like(p.Name.ToLower(), $"%{SearchQuery.ToLower()}%")
                                         || EF.Functions.Like(p.FirstName.ToLower(), $"%{SearchQuery.ToLower()}%")
                                         || EF.Functions.Like(p.LastName.ToLower(), $"%{SearchQuery.ToLower()}%"));
            }

            Doctors = await query.ToListAsync();
            return Page();
        }

    }
}
