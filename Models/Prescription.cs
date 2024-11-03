using System.ComponentModel.DataAnnotations;

namespace CNSVM.Models
{
    public class Prescription
    {
        [Key]
        public int Id { get; set; }
        public DateTime RequestDate { get; set; }

        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        // Relaciones
        public User Doctor { get; set; }
        public Patient Patient { get; set; }

        public ICollection<MedicamentPrescription> MedicamentPrescriptions { get; set; }
    }

}
