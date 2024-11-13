using CNSVM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using CNSVM.Data;
using Microsoft.EntityFrameworkCore;
using CNSVM.Models.ModelView;
using iText.Commons.Actions.Contexts;
using iText.Layout.Renderer;
using CNSVM.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace CNSVM.Pages.Patients
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly CnsvmDbContext _cnsvmDbContext;
        private readonly ReportService _reportService;

        public DetailsModel(CnsvmDbContext cnsvmDbContext, ReportService report)
        {
            _cnsvmDbContext = cnsvmDbContext;
            _reportService = report;
        }

        public List<MedicamentPrescription> MedicamentPrescription { get; set; }
        public PatientJ Paciente { get; set; }
        public int Edad { get; set; }
        public bool PacienteEncontrado { get; set; } = true;
        public List<MedicamentJustification> DeclinedMedicaments { get; set; }
        public User ActualDoctor { get; set; }

        public async Task OnGetAsync(int id)
        {
            var pacientes = LoadPatientsData();

            // Mensaje para verificar el ID que se está buscando
            Console.WriteLine($"Buscando paciente con Matricula: {id}");

            Paciente = pacientes?.FirstOrDefault(p => p.matricula == id);

            await Verified(id);

            if (Paciente == null)
            {
                PacienteEncontrado = false;
            }
            else
            {
                // Calcular la edad usando la fecha de nacimiento
                Edad = DateTime.Today.Year - Paciente.fechaNacimiento.Year;
                if (Paciente.fechaNacimiento > DateTime.Today.AddYears(-Edad))
                {
                    Edad--;
                }

                // Obtener los medicamentos rechazados
                DeclinedMedicaments = await GetDeclinedMedicaments(id);
            }
        }

        public async Task<IActionResult> OnGetDownloadReport(int id)
        {
            var pacientes = LoadPatientsData();

            var doctor = await GetDoctorFromClaim();
            var medicaments = await GetDeclinedMedicaments(id);
            var paciente = pacientes?.FirstOrDefault(p => p.matricula == id);
            // Lógica para generar y devolver el archivo PDF
            var pdfContent = _reportService.CreatePdf($"{doctor.Name} {doctor.FirstName} {doctor.LastName}", doctor.Specialty, paciente.nombre, medicaments);

            var fileName = $"{paciente.matricula}{paciente.nombre.Replace(" ","")}.pdf";
            return File(pdfContent, "application/pdf", fileName);
        }

        private List<PatientJ> LoadPatientsData()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "patients.json");
            var jsonString = System.IO.File.ReadAllText(filePath);

            // Configuración para deserialización con insensibilidad a mayúsculas y minúsculas
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<PatientJ>>(jsonString, options);
        }

        private async Task<User> GetDoctorFromClaim()
        {
            var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int doctorId = int.Parse(doctorIdClaim.Value);

            return await _cnsvmDbContext.User.FirstOrDefaultAsync(u => u.Id == doctorId);
        }

        private async Task<List<MedicamentJustification>> GetDeclinedMedicaments(int id)
        {
            var initialData = await (from mc in _cnsvmDbContext.MedicalCriterion
                                     join mp in _cnsvmDbContext.MedicamentPrescription
                                     on mc.MedicamentPrescriptionId equals mp.Id
                                     join med in _cnsvmDbContext.Medicament
                                     on mp.MedicamentId equals med.Id
                                     where mc.Criterion == 'R' && mp.id_historia == id
                                     select new { mc, mp, med })
                                .ToListAsync();

            return initialData
                   .GroupBy(x => new { x.mp.Id, x.med.Name })
                   .Select(groupedCriterion =>
                   {
                       var latestCriterion = groupedCriterion
                           .OrderByDescending(c => c.mc.CriterionDate)
                           .FirstOrDefault();
                       return new MedicamentJustification()
                       {
                           MedicamentName = groupedCriterion.Key.Name,
                           Justification = latestCriterion?.mc.CriterionReason
                       };
                   })
                   .ToList();
        }

        public async Task Verified(int id)
        {
            int cantidadMedicamentos = 0;

            foreach (var hist in Paciente.historiasClinicas)
            {
                cantidadMedicamentos += hist.medicamentos.Count;
            }

            MedicamentPrescription = await _cnsvmDbContext.MedicamentPrescription
                                    .Where(mp => mp.id_historia == id)
                                    .Include(mp => mp.Medicament)
                                    .ToListAsync();

            if (MedicamentPrescription.Count < cantidadMedicamentos)
            {
                await AddMedicationPrescription(id);
                MedicamentPrescription = await _cnsvmDbContext.MedicamentPrescription
                                    .Where(mp => mp.id_historia == id)
                                    .Include(mp => mp.Medicament)
                                    .ToListAsync();
            }
        }

        public async Task AddMedicationPrescription(int id)
        {
            // Carga todos los medicamentos y prescripciones existentes en memoria
            var allMedicaments = await _cnsvmDbContext.Medicament.ToListAsync();
            var existingPrescriptions = await _cnsvmDbContext.MedicamentPrescription
                                             .Where(mp => mp.id_historia == id)
                                             .ToListAsync();

            foreach (var hist in Paciente.historiasClinicas)
            {
                foreach (var med in hist.medicamentos)
                {
                    // Busca el medicamento en la lista cargada
                    var medicament = allMedicaments.FirstOrDefault(m => m.Name == med.nombreMedicamento);

                    if (medicament != null)
                    {
                        // Verifica si ya existe la prescripción en la lista de prescripciones
                        bool existe = existingPrescriptions.Any(mp => mp.MedicamentId == medicament.Id);

                        if (!existe)
                        {
                            // Crea una nueva prescripción y añádela al contexto
                            var medicamentPrescription = new MedicamentPrescription
                            {
                                id_historia = id,
                                MedicamentId = medicament.Id,
                                IdDoctor = hist.medicoId,
                                Status = 'P'
                            };

                            await _cnsvmDbContext.AddAsync(medicamentPrescription);
                        }
                    }
                }
            }

            await _cnsvmDbContext.SaveChangesAsync();
        }
    }

}
