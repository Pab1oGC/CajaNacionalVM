namespace CNSVM.Models
{
	public class MedicamentPrescription
	{
        public int Id { get; set; }
        public int PrescriptionId { get; set; }
        public int MedicamentId { get; set; }
        public char Status { get; set; }

        // Relaciones con otras entidades
        public Prescription Prescription { get; set; }
        public Medicament Medicament { get; set; }
    }

}
