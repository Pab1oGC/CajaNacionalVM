using Microsoft.AspNetCore.Authorization;
using CNSVM.Data;
using CNSVM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace CNSVM.Pages.Patients
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public List<PatientJ> Pacientes { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        public void OnGet()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "patients.json");

            if (System.IO.File.Exists(path))
            {
                var jsonString = System.IO.File.ReadAllText(path);
                Pacientes = JsonSerializer.Deserialize<List<PatientJ>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Ignora mayúsculas y minúsculas en los nombres de propiedades
                });
            }
        }
    }
}
