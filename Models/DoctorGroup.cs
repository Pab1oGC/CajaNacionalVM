using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNSVM.Models
{
    public class DoctorGroup
    {
        [Key]
        public int GroupId { get; set; }
        public int UserId { get; set; }

        // Relaciones
        [ForeignKey("GroupId")]
        public MedicalGroup? MedicalGroup { get; set; }
        public User? User { get; set; }
    }
}
