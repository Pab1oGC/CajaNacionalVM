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
        private readonly SupabaseService _supabaseService;
        public DetailsModel(CnsvmDbContext cnsvmDbContext, SupabaseService supabaseService) 
        {
            _cnsvmDbContext = cnsvmDbContext;
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
                PatientDetail = await _cnsvmDbContext.Patient.Where(p => p.Id == id).FirstOrDefaultAsync();
                PhotoPath = await _supabaseService.GetPublicImageUrl(PatientDetail!.Id);
                await GetPrescription(id);
            }
            catch (Exception)
            {
            }
        }


        
        public async Task GetPrescription(int PatientId)
        {

            Prescriptions = await _cnsvmDbContext.Prescription
                        .Where(p => p.PatientId == PatientId)
                        .Select(p => new Prescription
                        {
                            Id = p.Id,
                            RequestDate = p.RequestDate,
                            Doctor = new User  // Ajustamos la relaci�n si hay m�s de un doctor
                            {
                                Name = p.Doctor.Name,
                                LastName = p.Doctor.LastName,
                                FirstName = p.Doctor.FirstName,
                                Specialty = p.Doctor.Specialty,
                            },
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
