using System.ComponentModel.DataAnnotations;

namespace CNSVM.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }
        public string SuyName { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string DocumentNumber { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string ContactPhone { get; set; }
        public DateOnly? BirthDate { get; set; }

        // Relaciones
    }
}
