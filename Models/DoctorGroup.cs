using System.ComponentModel.DataAnnotations;

namespace CNSVM.Models
{
	public class DoctorGroup
	{
        [Key]
        public int GroupId { get; set; }
		public int UserId { get; set; }

		// Relaciones
		public MedicalGroup? MedicalGroup { get; set; }
		public User? User { get; set; }
	}

}
