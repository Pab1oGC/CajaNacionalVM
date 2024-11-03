using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNSVM.Models
{

    public class DoctorGroup
    {
        [Key, Column(Order = 0)]
        public int GroupId { get; set; }

        [Key, Column(Order = 1)]
        public int UserId { get; set; }

        // Relaciones
        [ForeignKey("GroupId")]
        public MedicalGroup? MedicalGroup { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
