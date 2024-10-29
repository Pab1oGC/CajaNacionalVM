using CNSVM.Data;
using CNSVM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Supabase.Gotrue;
using System.Text.RegularExpressions;

namespace CNSVM.Pages.Groups
{
    public class IndexModel : PageModel
    {
        private readonly CnsvmDbContext _context;

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        public IndexModel(CnsvmDbContext context)
        {
            _context = context;
        }

        public IEnumerable<MedicalGroup> MedicalGroup { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var query = _context.MedicalGroup.AsQueryable();

            // Filtrar resultados si hay una búsqueda
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                query = query.Where(g => EF.Functions.Like(g.Name, $"%{SearchQuery}%"));
            }

            MedicalGroup = await query.ToListAsync();
            return Page();
        }

    }
}

