using CNSVM.Data;
using CNSVM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CNSVM.Pages.Patients
{
    public class MedicalCriterionsModel : PageModel
    {
        private readonly CnsvmDbContext _context;

        public MedicalCriterionsModel(CnsvmDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string MedicamentName { get; set; }
        public string UserName { get; set; }

        public async Task<IActionResult> OnGetAsync(int doctorId, int prescriptionId)
        {

            try
            {
                // Obtener la prescripci�n del medicamento con PrescriptionId
                var prescription = await _context.MedicamentPrescription
                    .Include(mp => mp.Medicament) // Incluir la relaci�n con Medicament
                    .FirstOrDefaultAsync(mp => mp.PrescriptionId == prescriptionId);

                // Verificar si se encontr� la prescripci�n y el medicamento relacionado
                if (prescription != null && prescription.Medicament != null)
                {
                    // Asignar el nombre del medicamento
                    MedicamentName = prescription.Medicament.Name ?? "Nombre del medicamento no disponible";
                }
                else
                {
                    MedicamentName = "No se encontr� el medicamento";
                }

                // Obtener los datos del doctor/usuario relacionado
                var user = await _context.User.FindAsync(doctorId);
                if (user == null)
                {
                    return NotFound("No se encontr� el doctor especificado.");
                }

                // Asignar el nombre del doctor/usuario
                UserName = user.Username ?? "Nombre no disponible";

                return Page();
            }
            catch (Exception ex)
            {
                // Capturar y loguear el error
                Console.WriteLine("Error al obtener los datos: " + ex.Message);
                return StatusCode(500, "Se produjo un error al acceder a la base de datos.");
            }
        }

        public async Task<IActionResult> OnPostAsync(char doctorVote, string justification, int doctorId, int prescriptionId)
        {
            // Validaci�n simple: Verificar que se haya seleccionado un voto
            if (doctorVote != 'S' && doctorVote != 'N')
            {
                ModelState.AddModelError(string.Empty, "Debe seleccionar una opci�n de voto.");
                return Page(); // Devolver la p�gina con el error
            }

            // Verificar que la prescripci�n y su relaci�n con el medicamento existen
            var prescription = await _context.MedicamentPrescription
                .Include(mp => mp.Medicament)
                .FirstOrDefaultAsync(mp => mp.PrescriptionId == prescriptionId);

            if (prescription == null || prescription.Medicament == null)
            {
                ModelState.AddModelError(string.Empty, "No se encontr� la prescripci�n o el medicamento especificado.");
                return Page(); // Devolver la p�gina con el error si no se encuentra
            }

            // Crear un nuevo criterio m�dico basado en el voto
            var medicalCriterion = new MedicalCriterion
            {
                UserId = doctorId,
                MedicamentPrescriptionId = prescription.Id, // Usar el ID de la relaci�n MedicamentPrescription
                Criterion = doctorVote, // Guardar 'S' para s� y 'N' para no
                CriterionReason = doctorVote == 'N' ? justification : null,  // Justificaci�n solo si es "No"
                CriterionDate = DateTime.Now
            };

            // Guardar el criterio en la base de datos
            _context.MedicalCriterion.Add(medicalCriterion);
            await _context.SaveChangesAsync();
            return Page();
        }
    }
}

