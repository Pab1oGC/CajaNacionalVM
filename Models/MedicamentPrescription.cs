using System.ComponentModel.DataAnnotations;

namespace CNSVM.Models
{
	public class MedicamentPrescription
	{
        [Key]
        public int Id { get; set; }
        public int PrescriptionId { get; set; }
        public int MedicamentId { get; set; }
        public char Status { get; set; }

        // Relaciones con otras entidades
        public Prescription Prescription { get; set; }
        public Medicament Medicament { get; set; }
        public ICollection<MedicalCriterion> MedicalCriteria { get; set; }
    }

}
