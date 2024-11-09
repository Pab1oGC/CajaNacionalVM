using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNSVM.Models
{
    public class MedicamentPrescription
    {
        [Key]
        public int Id { get; set; }

        public int id_historia { get; set; }
        public int MedicamentId { get; set; }
        public int IdDoctor { get; set; }
        public char Status { get; set; }


        // Relaciones
        public ClinicalHistory? Prescription { get; set; }
        public Medicament? Medicament { get; set; }
        public ICollection<MedicalCriterion>? MedicalCriterion { get; set; }
    }

}
