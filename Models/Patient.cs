using System.ComponentModel.DataAnnotations;

namespace CNSVM.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string DocumentNumber { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string ContactPhone { get; set; }
    }
}
