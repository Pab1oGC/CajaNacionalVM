using CNSVM.Data;
using CNSVM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

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

        public async Task OnGetAsync(int page = 1)
        {

            Medicaments = await _cnsvmDbContext.Medicament.ToListAsync();
        }
    }
}
