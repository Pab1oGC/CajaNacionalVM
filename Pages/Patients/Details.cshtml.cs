using CNSVM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using CNSVM.Data;
using Microsoft.EntityFrameworkCore;

namespace CNSVM.Pages.Patients
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly CnsvmDbContext _cnsvmDbContext;
        public DetailsModel(CnsvmDbContext cnsvmDbContext)
        {
            _cnsvmDbContext = cnsvmDbContext;
        }
        public List<MedicamentPrescription> MedicamentPrescription { get; set; }
        public PatientJ Paciente { get; set; }
        public int Edad { get; set; }
        public bool PacienteEncontrado { get; set; } = true; // Para manejar si se encontró el paciente

        public async Task OnGet(int id)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "patients.json");
            var jsonString = System.IO.File.ReadAllText(filePath);

            // Configuración para deserialización con insensibilidad a mayúsculas y minúsculas
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var pacientes = JsonSerializer.Deserialize<List<PatientJ>>(jsonString, options);

            // Mensaje para verificar el ID que se está buscando
            Console.WriteLine($"Buscando paciente con Matricula: {id}");

            // Encuentra el paciente por Matricula (id)
            Paciente = pacientes?.FirstOrDefault(p => p.Matricula == id);
            
            await Verified(id);

            if (Paciente == null)
            {
                PacienteEncontrado = false;
            }
            else
            {
                // Calcular la edad usando la fecha de nacimiento
                var today = DateTime.Today;
                Edad = today.Year - Paciente.FechaNacimiento.Year;

                // Ajustar si el cumpleaños aún no ha ocurrido este año
                if (Paciente.FechaNacimiento > today.AddYears(-Edad))
                {
                    Edad--;
                }
            }
            
        }
        public async Task Verified(int id)
        {
            
            int cantidadMedicamentos = 0;

            foreach (var hist in Paciente.historias_clinicas)
            {
                cantidadMedicamentos += hist.Medicamentos.Count;
            }

            MedicamentPrescription = await _cnsvmDbContext.MedicamentPrescription
                                    .Where(mp=> mp.id_historia == id)
                                    .Include(mp => mp.Medicament)
                                    .ToListAsync();          
            if (MedicamentPrescription.Count < cantidadMedicamentos)    
            {
                await AddMedicationPrescription(id);
            }          
        }
        public async Task AddMedicationPrescription(int id)
        {
            // Cargar todos los medicamentos y prescripciones existentes una vez
            var allMedicaments = await _cnsvmDbContext.Medicament.ToListAsync();
            

            foreach (var hist in Paciente.historias_clinicas)
            {
                foreach (var med in hist.Medicamentos)
                {
                    // Busca el medicamento en la lista cargada en memoria
                    var medicament = allMedicaments.FirstOrDefault(m => m.Name == med.Nombre);

                    if (medicament != null)
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

            await _cnsvmDbContext.SaveChangesAsync();
        }

    }
}
