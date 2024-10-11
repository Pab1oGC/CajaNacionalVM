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
        private readonly ErpcnsDbContext _erpcnsDbContext;
        public IndexModel(ErpcnsDbContext erpcnsDbContext)
        {
            _erpcnsDbContext = erpcnsDbContext;
        }
        public IEnumerable<Patient> Patients { get; set; }
        public async Task OnGet()
        {
            Patients = await _erpcnsDbContext.Patient.ToListAsync();
        }
    }
}
