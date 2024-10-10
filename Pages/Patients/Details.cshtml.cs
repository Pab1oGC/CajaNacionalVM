using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VeriMedCNS.Data;
using VeriMedCNS.Models;
using VeriMedCNS.Services;

namespace VeriMedCNS.Pages.Patients
{
    public class DetailsModel : PageModel
    {
        private readonly CnsvmDbContext _cnsvmDbContext ;
        private readonly SupabaseService _supabaseService;
        private readonly ErpcnsDbContext erpcnsDbContext_;
        public DetailsModel(CnsvmDbContext cnsvmDbContext, SupabaseService supabaseService, ErpcnsDbContext erpcnsDbContext) 
        {
            _cnsvmDbContext = cnsvmDbContext;
            _supabaseService = supabaseService;
            erpcnsDbContext_ = erpcnsDbContext;
        }
        public IEnumerable<MedicationRequest> MedicationRequests { get; set; }
        public Patient PatientDetail { get; set; }
        public string PhotoPath { get; set; }
        public async Task OnGet(int id)
        {
            try
            {
                PatientDetail = await erpcnsDbContext_.Patient.Where(x => x.PatientId == id).FirstOrDefaultAsync();
                PhotoPath = await _supabaseService.GetPublicImageUrl(PatientDetail!.PatientId);
                await GetMedicationRequest(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public async Task GetMedicationRequest(int patientID ) 
        {
            MedicationRequests = await _cnsvmDbContext.MedicationRequest.Where(u =>u.PatientId == patientID).ToListAsync();
        }
    }
}
