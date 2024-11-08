using CNSVM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

namespace CNSVM.Pages.Patients
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        public PatientJ Paciente { get; set; }
        public int Edad { get; set; }
        public bool PacienteEncontrado { get; set; } = true; // Para manejar si se encontr� el paciente

        public void OnGet(int id)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "patients.json");
            var jsonString = System.IO.File.ReadAllText(filePath);

            // Configuraci�n para deserializaci�n con insensibilidad a may�sculas y min�sculas
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var pacientes = JsonSerializer.Deserialize<List<PatientJ>>(jsonString, options);

            // Mensaje para verificar el ID que se est� buscando
            Console.WriteLine($"Buscando paciente con Matricula: {id}");

            // Encuentra el paciente por Matricula (id)
            Paciente = pacientes?.FirstOrDefault(p => p.Matricula == id);

            if (Paciente == null)
            {
                PacienteEncontrado = false;
            }
            else
            {
                // Calcular la edad usando la fecha de nacimiento
                var today = DateTime.Today;
                Edad = today.Year - Paciente.FechaNacimiento.Year;

                // Ajustar si el cumplea�os a�n no ha ocurrido este a�o
                if (Paciente.FechaNacimiento > today.AddYears(-Edad))
                {
                    Edad--;
                }
            }
        }
    }
}
