using CNSVM.Data;
using CNSVM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.InteropServices;

namespace CNSVM.Pages.Groups
{
    public class CreateModel : PageModel
    {
        private readonly CnsvmDbContext _cnsvmDbContext;
        public CreateModel(CnsvmDbContext db) 
        {
            _cnsvmDbContext = db;
        }
        [BindProperty]
        public MedicalGroup Group { get; set; }
        public IEnumerable<User> Doctors { get; set; }
        public void OnGet()
        {
            Doctors = _cnsvmDbContext.User;
        }
    }
}
