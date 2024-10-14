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


        private readonly CnsvmDbContext _cnsvmDbContext;

        public MedicalCriterionsModel(CnsvmDbContext context)
        {
            _cnsvmDbContext = context ?? throw new ArgumentNullException(nameof(context));

        }

        [BindProperty]
        public string MedicamentName { get; set; }
        public string UserName { get; set; }
        public MedicamentPrescription medicamentPrescription { get; set; }

        public IEnumerable<MedicalCriterion> medicalCriteria { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                medicamentPrescription = await _cnsvmDbContext.MedicamentPrescription
                    .Where(mp => mp.Id == id)
                    .Include(mp => mp.Medicament)
                    .Include(mp => mp.Prescription)
                    .FirstOrDefaultAsync();


                

                if (medicamentPrescription != null && medicamentPrescription.Medicament != null)
                {
                    MedicamentName = medicamentPrescription.Medicament.Name ?? "Nombre no disponible";

                }
                else
                {
                    MedicamentName = "No se encontr� el medicamento";
                }

                string username = Request.Cookies["Username"];

                var user = await _cnsvmDbContext.User.Where(u=> u.Username == username).FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound("No se encontr� el doctor especificado.");
                }


                UserName = user.Username ?? "Nombre no disponible";

                medicalCriteria = await _cnsvmDbContext.MedicalCriterion
                                .Where(mc => mc.MedicamentPrescriptionId == id)
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
            var prescription = await _cnsvmDbContext.MedicamentPrescription


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
            _cnsvmDbContext.MedicalCriterion.Add(medicalCriterion);
            await _cnsvmDbContext.SaveChangesAsync();
            return Page();

        }
    }



}

