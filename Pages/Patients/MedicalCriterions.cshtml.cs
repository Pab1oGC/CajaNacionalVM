using CNSVM.Data;
using CNSVM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace CNSVM.Pages.Patients
{
    [Authorize]
    public class MedicalCriterionsModel : PageModel
    {


        private readonly CnsvmDbContext _cnsvmDbContext;
  

        public MedicalCriterionsModel(CnsvmDbContext cnsvmDbContext)
        {
            _cnsvmDbContext = cnsvmDbContext;
           

        }

        // Propiedades para mostrar los datos
        public string MedicamentName { get; set; }
        public string UserName { get; set; }
        public string DoctorSpecialty { get; set; } // Nueva propiedad para la especialidad del doctor
        public string PharmaceuticalForm { get; set; }

        // Propiedad para recibir el voto del usuario
        [BindProperty]
        public string DoctorVote { get; set; }

        [BindProperty]
        public string ? Justification { get; set; } // Justificación en caso de rechazo

        public int UserId { get; set; }
        public int MedicamentPrescriptionId { get; set; }
        public IEnumerable<MedicamentPrescription> medicamentPrescriptions { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                medicamentPrescriptions = await _cnsvmDbContext.MedicamentPrescription.
                                        Include(mp => mp.Medicament).
                                        Include(mp => mp.MedicalCriterion)!
                                            .ThenInclude(mc => mc.User).
                                        Where(mp => mp.Id == id).
                                        ToListAsync(); 
                // Recuperar el ID del médico desde los claims (guardado en el login)
                var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (doctorIdClaim == null)
                {
                    ModelState.AddModelError("", "No se pudo recuperar el ID del médico.");
                    return Page();
                }

                int doctorId = int.Parse(doctorIdClaim.Value); // Convertir el ID de string a int
                Console.WriteLine("Doctor ID obtenido del claim: " + doctorId);

                // Consultar la tabla User para obtener el nombre y la especialidad del doctor
                var doctor = await _cnsvmDbContext.User.FirstOrDefaultAsync(u => u.Id == doctorId);
                if (doctor != null)
                {
                    UserName = $"{doctor.Name} {doctor.LastName}";
                    DoctorSpecialty = doctor.Specialty; // Asumimos que Specialty contiene la especialidad
                    Console.WriteLine($"Doctor encontrado: {UserName}, Especialidad: {DoctorSpecialty}");
                }
                else
                {
                    throw new Exception("Doctor no encontrado en la base de datos.");
                }

                // Obtener los detalles del MedicamentPrescription usando el medicamentPrescriptionId


                if (medicamentPrescriptions == null)
                {
                    ModelState.AddModelError("", "No se encontró la prescripción del medicamento.");
                    return Page();
                }
                MedicamentPrescriptionId = id;

                // Obtener los detalles del medicamento
                var medicament = await _cnsvmDbContext.Medicament
                    .FirstOrDefaultAsync(m => m.Id == medicamentPrescriptions.FirstOrDefault().MedicamentId);

                if (medicament != null)
                {
                    MedicamentName = medicament.Name;
                    PharmaceuticalForm = medicament.PharmaceuticalForm;
                    Console.WriteLine($"Medicamento encontrado: {MedicamentName}, Forma Farmacéutica: {PharmaceuticalForm}");
                }
                else
                {
                    ModelState.AddModelError("", "No se encontró el medicamento.");
                    return Page();
                }

                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ModelState.AddModelError("", "Ocurrió un error inesperado al cargar los datos.");
                return Page();
            }

        }

        // Método OnPost para procesar el voto y guardarlo en la base de datos
        public async Task<IActionResult> OnPostAsync(int medicamentPrescriptionId)
        {
            // Recuperar el ID del médico desde los claims (almacenado en el login)
            var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (doctorIdClaim == null)
            {
                ModelState.AddModelError("", "No se pudo recuperar el ID del médico.");
                return Page();
            }

            int doctorId = int.Parse(doctorIdClaim.Value); // Convertir el ID de string a int
            Console.WriteLine("Doctor ID obtenido del claim: " + doctorId);

            try
            {
                // Validar que se haya seleccionado un voto
                if (string.IsNullOrEmpty(DoctorVote) || (DoctorVote != "A" && DoctorVote != "R"))
                {
                    ModelState.AddModelError("", "Debe seleccionar una opción válida.");
                    return Page();  // Retornar si no hay un voto válido
                }

                // Si el doctor vota "No" (Rechazado), debe haber una justificación
                if (DoctorVote == "R" && string.IsNullOrWhiteSpace(Justification))
                {
                    ModelState.AddModelError("Justification", "Debe proporcionar una justificación si rechaza el medicamento.");
                    medicamentPrescriptions = await _cnsvmDbContext.MedicamentPrescription.
                                        Include(mp => mp.Medicament).
                                        Include(mp => mp.MedicalCriterion)!
                                            .ThenInclude(mc => mc.User).
                                        Where(mp => mp.Id == medicamentPrescriptionId).
                                        ToListAsync();
                    return Page();
                }

                // Obtener la prescripción del medicamento
                var medicamentPrescription = await _cnsvmDbContext.MedicamentPrescription
                    .FirstOrDefaultAsync(mp => mp.Id == medicamentPrescriptionId);

                if (medicamentPrescription == null)
                {
                    ModelState.AddModelError("", "No se encontró la prescripción de medicamento especificada.");
                    return Page();
                }

                // Crear y guardar el voto en la tabla `MedicalCriterion`
                var medicalCriterion = new MedicalCriterion
                {
                    UserId = doctorId,  // Usar el ID del doctor logueado
                    MedicamentPrescriptionId = medicamentPrescription.Id,  // Asociar con MedicamentPrescription
                    Criterion = DoctorVote[0],  // Guardar 'A' para Aprobado o 'R' para Rechazado
                    CriterionReason = DoctorVote == "R" ? DeleteExtraSpaces(Justification!.Trim()) : null,  // Guardar justificación solo si es "No"
                    CriterionDate = DateTime.Now  // Fecha y hora actuales
                };

                // Agregar el voto a la base de datos
                await _cnsvmDbContext.MedicalCriterion.AddAsync(medicalCriterion);

                medicamentPrescription.Status = (DoctorVote == "A" && medicamentPrescription.Status != 'R') ? 'A' : 'R';
                _cnsvmDbContext.MedicamentPrescription.Update(medicamentPrescription);


                // Guardar los cambios en la base de datos
                await _cnsvmDbContext.SaveChangesAsync();
                Console.WriteLine("Voto guardado exitosamente en la base de datos.");

                // Redirigir a la misma página con un querystring que indique éxito (para mostrar el modal)
                return RedirectToPage(new { showModal = true });
            }
            catch (Exception ex)
            {
                // Manejar errores y mostrar mensajes de error
                Console.WriteLine("Error al guardar el voto: " + ex.Message);
                ModelState.AddModelError("", $"Error al guardar el voto: {ex.Message}");
                return Page();
            }
        }
        string DeleteExtraSpaces(string justification)
        {
            return Regex.Replace(justification, @"\s+", " ");
        }
    }
}
