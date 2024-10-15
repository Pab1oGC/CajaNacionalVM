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
        //DrNico
        //12909506

        private readonly CnsvmDbContext _context;

        public MedicalCriterionsModel(CnsvmDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }

        [BindProperty]
        public string MedicamentName { get; set; }
        public string UserName { get; set; }
        public string DoctorSpecialty { get; set; }  // Nueva propiedad para la especialidad del m�dico
        public string PrescriptionDate { get; set; }  // Nueva propiedad para la fecha de la prescripci�n

        public List<MedicalCriterionViewModel> MedicalCriterions { get; set; }
        

        public async Task<IActionResult> OnGetAsync(int doctorId, int prescriptionId)
        {
            try
            {
                var prescription = await _context.MedicamentPrescription
                    .Include(mp => mp.Medicament) // Incluimos el medicamento relacionado
                    .Include(mp => mp.Prescription) // Incluimos la prescripci�n relacionada
                    .FirstOrDefaultAsync(mp => mp.PrescriptionId == prescriptionId);

                if (prescription != null && prescription.Medicament != null)
                {
                    MedicamentName = prescription.Medicament.Name ?? "Nombre no disponible";
                }
                else
                {
                    MedicamentName = "No se encontr� el medicamento";
                }

                var user = await _context.User.FindAsync(doctorId);

                if (user == null)
                {
                    return NotFound("No se encontr� el doctor especificado.");
                }

                UserName = user.Username ?? "Nombre no disponible";

                // Obtenemos los criterios m�dicos para el MedicamentPrescriptionId
                MedicalCriterions = await _context.MedicalCriterion
                    .Where(mc => mc.MedicamentPrescriptionId == prescription.Id) // Filtramos por MedicamentPrescriptionId
                    .Select(mc => new MedicalCriterionViewModel
                    {
                        DoctorFullName = mc.User.Name + " " + mc.User.LastName, // Nombre completo del m�dico
                        Criterion = mc.Criterion, // Voto ('S' o 'N')
                        CriterionReason = mc.CriterionReason // Raz�n del voto (si existe)
                    })
                    .ToListAsync();

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

    public class MedicalCriterionViewModel
    {
        public string DoctorFullName { get; set; }
        public char Criterion { get; set; }
        public string CriterionReason { get; set; }
    }
}

