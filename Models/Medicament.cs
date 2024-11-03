using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNSVM.Models
{
    public class Medicament
    {
        [Key]
        public int Id { get; set; }

        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? PharmaceuticalForm { get; set; }
        public string? Consentration { get; set; }
        public string? Clasific { get; set; }
        public char? RestrictedUse { get; set; }

        // Relaciones
        public ICollection<MedicamentPrescription> MedicamentPrescriptions { get; set; }
    }
}
