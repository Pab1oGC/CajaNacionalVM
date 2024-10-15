using System.ComponentModel.DataAnnotations;

namespace CNSVM.Models
{
	public class User
	{
        [Key]
        public int Id { get; set; }
		public string Username { get; set; }
		public string PasswordHash { get; set; }
		public string Role { get; set; }
		public DateTime CreatedAt { get; set; }
		public string Status { get; set; }
		public string Name { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Specialty { get; set; }

		// Relaciones
		public ICollection<MedicalCriterion>? MedicalCriteria { get; set; }
		public ICollection<MedicalGroupAudit>? MedicalGroupAudits { get; set; }
		public ICollection<Prescription>? Prescriptions { get; set; }
	}

}
