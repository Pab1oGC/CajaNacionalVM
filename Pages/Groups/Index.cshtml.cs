using CNSVM.Data;
using CNSVM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Supabase.Gotrue;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace CNSVM.Pages.Groups
{
    public class IndexModel : PageModel
    {
        private readonly CnsvmDbContext _context;

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        public IndexModel(CnsvmDbContext context)
        {
            _context = context;
        }



        public IList<MedicalGroupViewModel> MedicalGroup { get; set; }
        public async Task<IActionResult> OnPostCreateAsync(MedicalGroup medicalGroup)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Asume que tienes acceso al usuario logueado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Obtén el ID del usuario actual
            medicalGroup.CreatedBy = int.Parse(userId); // Guarda el ID del creador

            _context.MedicalGroup.Add(medicalGroup);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
        public async Task OnGetAsync()
        {
            MedicalGroup = await _context.MedicalGroup
            .Include(g => g.DoctorGroups)  // Incluye la relación con DoctorGroups
                .ThenInclude(dg => dg.User) // Incluye la relación con User para acceder al nombre del creador
            .Select(g => new MedicalGroupViewModel
            {
                Id = g.Id,
                Name = g.Name,
                CreatedAt = g.CreatedAt,
                Status = g.Status,
                CreatedBy = g.DoctorGroups
                    .Where(dg => dg.UserId == g.CreatedBy)
                    .Select(dg => dg.User.Name + " " + dg.User.FirstName + " " + dg.User.LastName)
                    .FirstOrDefault() ?? "Desconocido"
            })
            .ToListAsync();
        }

        public class MedicalGroupViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime CreatedAt { get; set; }
            public string Status { get; set; }
            public string CreatedBy { get; set; } // Propiedad para mostrar el nombre del creador
        }

    }
}

