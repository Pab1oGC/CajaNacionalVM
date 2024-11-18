using Microsoft.AspNetCore.Authorization;
using CNSVM.Data;
using CNSVM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace CNSVM.Pages.Patients
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public ICollection<PatientJ> Pacientes { get; set; } = new List<PatientJ>();

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        public void OnGet()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "patients.json");

            if (System.IO.File.Exists(path))
            {
                var jsonString = System.IO.File.ReadAllText(path);
                var allPacientes = JsonSerializer.Deserialize<List<PatientJ>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (!string.IsNullOrEmpty(SearchQuery))
                {
                    // Convertir carnetIdentidad a string para buscar
                    Pacientes = allPacientes
                        .Where(p => p.nombre.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)
                                    || p.carnetIdentidad.ToString().Contains(SearchQuery))
                        .Take(10) // Limitar a 10 resultados
                        .ToList();
                }
                else
                {
                    // Limitar a 10 si no hay búsqueda
                    Pacientes = allPacientes.Take(10).ToList();
                }
            }
        }


    }
}
