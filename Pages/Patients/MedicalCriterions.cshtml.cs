using CNSVM.Data;
using CNSVM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CNSVM.Pages.Patients
{
    public class MedicalCriterionsModel : PageModel
    {
        private readonly CnsvmDbContext _context;

        public MedicalCriterionsModel(CnsvmDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [BindProperty]
        public string MedicamentName { get; set; }
        public string UserName { get; set; }

        public List<MedicalCriterionViewModel> MedicalCriterions { get; set; }

        public async Task<IActionResult> OnGetAsync(int doctorId, int prescriptionId)
        {
            try
            {
                var prescription = await _context.MedicamentPrescription
                    .Include(mp => mp.Medicament) // Incluimos el medicamento relacionado
                    .Include(mp => mp.Prescription) // Incluimos la prescripción relacionada
                    .FirstOrDefaultAsync(mp => mp.PrescriptionId == prescriptionId);

                if (prescription != null && prescription.Medicament != null)
                {
                    MedicamentName = prescription.Medicament.Name ?? "Nombre no disponible";
                }
                else
                {
                    MedicamentName = "No se encontró el medicamento";
                }

                var user = await _context.User.FindAsync(doctorId);
                if (user == null)
                {
                    return NotFound("No se encontró el doctor especificado.");
                }

                UserName = user.Username ?? "Nombre no disponible";

                // Obtenemos los criterios médicos para el paciente de la prescripción
                var patientId = prescription.Prescription.PatientId;

                MedicalCriterions = await _context.MedicalCriterion
                    .Where(mc => mc.MedicamentPrescription.Prescription.PatientId == patientId)
                    .Select(mc => new MedicalCriterionViewModel
                    {
                        DoctorFullName = mc.User.Name + " " + mc.User.LastName,
                        Criterion = mc.Criterion,
                        CriterionReason = mc.CriterionReason
                    })
                    .ToListAsync();

                return Page();
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Error al acceder a la base de datos.");
            }
        }

        public async Task<IActionResult> OnPostAsync(string doctorVote, string justification, int doctorId, int prescriptionId)
        {
            if (string.IsNullOrEmpty(doctorVote))
            {
                ModelState.AddModelError(string.Empty, "Debe seleccionar una opción de voto.");
                return Page();
            }

            var prescription = await _context.MedicamentPrescription
                .Include(mp => mp.Medicament)
                .FirstOrDefaultAsync(mp => mp.PrescriptionId == prescriptionId);

            if (prescription == null || prescription.Medicament == null)
            {
                ModelState.AddModelError(string.Empty, "No se encontró la prescripción o el medicamento especificado.");
                return Page();
            }

            var medicalCriterion = new MedicalCriterion
            {
                UserId = doctorId,
                MedicamentPrescriptionId = prescription.Id,
                Criterion = doctorVote == "yes" ? "S" : "N",
                CriterionReason = doctorVote == "no" ? justification : null,
                CriterionDate = DateTime.Now
            };

            _context.MedicalCriterion.Add(medicalCriterion);
            await _context.SaveChangesAsync();
            return RedirectToPage(); // Recargar la página después de guardar el voto
        }
    }

    public class MedicalCriterionViewModel
    {
        public string DoctorFullName { get; set; }
        public string Criterion { get; set; }
        public string CriterionReason { get; set; }
    }
}
