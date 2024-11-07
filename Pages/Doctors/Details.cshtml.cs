using CNSVM.Data;
using CNSVM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CNSVM.Pages.Doctors
{
    public class DetailsModel : PageModel
    {
        private readonly CnsvmDbContext _cnsvmDbContext;

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }
        public DetailsModel(CnsvmDbContext cnsvmDbContext)
        {
            _cnsvmDbContext = cnsvmDbContext;
        }

        public MedicalGroup MedicalGroup { get; set; }
        public User Doctordetail { get; set; }
       
        public DoctorGroup doctorGroup { get; set; }

        public string doctorG;

        

        public async Task Onget(int id)
        {
            try
            {
                

                Doctordetail = await _cnsvmDbContext.User.Where(p => p.Id == id).FirstOrDefaultAsync();
                await GetMedicalGroup(id);   
            }
            catch (Exception ex) 
            {
            }

        }
        public async Task GetMedicalGroup(int DoctorId)
        {
            try
            {
                 doctorG = await _cnsvmDbContext.DoctorGroup
                .Where(d => d.UserId == DoctorId)
                .Include(d => d.MedicalGroup)
                .Select(d => d.MedicalGroup.Name)
                .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
            }
            
               


        }


    }
}
