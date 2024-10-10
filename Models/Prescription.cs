using System.ComponentModel.DataAnnotations;

namespace CNSVM.Models
{
	public class Prescription
	{
        [Key]
        public int Id { get; set; }
        public DateTime RequestDate { get; set; }
        public int PatientId { get; set; }

	}

}
