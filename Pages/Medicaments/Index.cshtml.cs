using CNSVM.Data;
using CNSVM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CNSVM.Pages.Medicaments
{
    public class IndexModel : PageModel
    {
        private readonly CnsvmDbContext _cnsvmDbContext;
        public IndexModel(CnsvmDbContext cnsvmDbContext)
        {
            _cnsvmDbContext = cnsvmDbContext;
        }

        public const int PageSize = 10;
        public int TotalMedicaments { get; set; }
        public int CurrentPage { get; set; }
        public List<Medicament> Medicaments { get; set; }

        public async Task OnGetAsync(int page = 1)
        {
            TotalMedicaments = await _cnsvmDbContext.Medicament.CountAsync();
            CurrentPage = page < 1 ? 1 : page;

            Medicaments = await _cnsvmDbContext.Medicament
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
