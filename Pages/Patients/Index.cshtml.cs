using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VeriMedCNS.Models;
using VeriMedCNS.Data;

namespace VeriMedCNS.Pages.Patients
{
    public class IndexModel : PageModel
    {
        private readonly ErpcnsDbContext _erpcnsDbContext;
        public IndexModel(ErpcnsDbContext erpcnsDbContext) 
        {
            _erpcnsDbContext = erpcnsDbContext;
        } 
        public List<Patient> Patients { get; set; }
        public void OnGet()
        {
            Patients = _erpcnsDbContext.Patient.ToList();
        }
    }
}
