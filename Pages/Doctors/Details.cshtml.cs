using CNSVM.Data;
using CNSVM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CNSVM.Pages.Doctors
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly CnsvmDbContext _cnsvmDbContext;

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }
        public DetailsModel(CnsvmDbContext cnsvmDbContext)
        {
            _cnsvmDbContext = cnsvmDbContext;
        }

        public User Doctordetail { get; set; }
       
        public string doctorG;

        public async Task Onget(int id)
        {
            try
            {
                
                Doctordetail = await _cnsvmDbContext.User.Where(p => p.Id == id).FirstOrDefaultAsync();
   
            }
            catch (Exception ex) 
            {
                throw ex;
            }

        }
        
    }
}
