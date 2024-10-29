using System.ComponentModel.DataAnnotations;

namespace CNSVM.Models
{
	public class MedicalGroup
	{
        [Key]
        public int Id { get; set; }
		public string? Name { get; set; }
		public int CreatedBy { get; set; }
		public DateTime CreatedAt { get; set; }
		public string? Status { get; set; }

		// Relaciones
		public ICollection<DoctorGroup>? DoctorGroups { get; set; }
		//public ICollection<MedicalGroupAudit>? MedicalGroupAudits { get; set; }
	}

}
