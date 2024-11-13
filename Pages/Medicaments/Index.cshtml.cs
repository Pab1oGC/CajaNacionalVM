using CNSVM.Data;
using CNSVM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CNSVM.Pages.Medicaments
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly CnsvmDbContext _cnsvmDbContext;
        public IndexModel(CnsvmDbContext cnsvmDbContext)
        {
            _cnsvmDbContext = cnsvmDbContext;
        }

        public List<Medicament> Medicaments { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public async Task OnGetAsync(int page = 1)
        {
            var query = _cnsvmDbContext.Medicament.AsQueryable();

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(m => EF.Functions.Like(m.Name, $"%{SearchTerm}%"));
            }

            Medicaments = await query.ToListAsync();
        }
    }
}
