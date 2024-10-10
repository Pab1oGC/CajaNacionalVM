using CNSVM.Data;
using CNSVM.Models;
using CNSVM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CNSVM.Pages.Patients
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        //Contructor base
        private readonly CnsvmDbContext _cnsvmDbContext;
        private readonly ErpcnsDbContext _erpcnsDbContext;
        private readonly SupabaseService _supabaseService;
        public DetailsModel(CnsvmDbContext cnsvmDbContext,ErpcnsDbContext erpcnsDbContext, SupabaseService supabaseService) 
        {
            _cnsvmDbContext = cnsvmDbContext;
            _erpcnsDbContext = erpcnsDbContext;
            _supabaseService = supabaseService;
        }
        //the variables
        public IEnumerable<Prescription> Prescriptions { get; set; }
        public Patient PatientDetail { get; set; }
        public string PhotoPath { get; set; }
        //The Methods
        public async Task OnGet(int id)
        {
            try
            {
                PatientDetail = await _erpcnsDbContext.Patient.Where(p => p.PatientId == id).FirstOrDefaultAsync();
                PhotoPath = await _supabaseService.GetPublicImageUrl(PatientDetail!.PatientId);
                await GetPrescription(id);
            }
            catch (Exception)
            {
            }
        }
        //in this method get patients Prescriptions
        
        public async Task GetPrescription(int PatientId)
        {
            Prescriptions = await _cnsvmDbContext.Prescription
                            .Where(p => p.PatientId == PatientId)
                            .Select(p => new Prescription
                            {
                                Id = p.Id,
                                RequestDate = p.RequestDate,
                                MedicamentPrescriptions = p.MedicamentPrescriptions.Select(mp => new MedicamentPrescription
                                {
                                    Id = mp.Id,
                                    Status = mp.Status,
                                    Medicament = new Medicament
                                    {
                                        Name = mp.Medicament.Name,
                                        PharmaceuticalForm = mp.Medicament.PharmaceuticalForm
                                    }
                                }).ToList()
                            })
                            .ToListAsync();
        }

    }
}
