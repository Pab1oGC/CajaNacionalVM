using CNSVM.Data;
using CNSVM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CNSVM.Pages.Medicaments
{
    public class DetailsModel : PageModel
    {
        private readonly CnsvmDbContext _cnsvmDbContext;
        public DetailsModel(CnsvmDbContext cnsvmDbContext)
        {
            _cnsvmDbContext = cnsvmDbContext;
        }

        public Medicament MedicamentDetail { get; set; }
        public List<MedicalCriterion> MedicalCriteria { get; set; }
        public double ApprovalPercentage { get; set; }

        public async Task OnGetAsync(int id)
        {
            // Obtener los detalles del medicamento
            MedicamentDetail = await _cnsvmDbContext.Medicament
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            // Obtener todos los criterios médicos (votos) asociados al medicamento
            MedicalCriteria = await _cnsvmDbContext.MedicalCriterion
                .Where(mc => mc.MedicamentPrescription.MedicamentId == id)
                .Include(mc => mc.User) // Incluir el doctor
                .ToListAsync();

            // Calcular el porcentaje de aprobación (votos "A")
            
            var totalVotes = MedicalCriteria.Count();
            var approvedVotes = MedicalCriteria.Count(mc => mc.Criterion == 'A'); // Cambié "A" por 'A'

            ApprovalPercentage = totalVotes > 0 ? (double)approvedVotes / totalVotes * 100 : 0;

        }
    }
}
