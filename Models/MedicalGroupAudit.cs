using System.ComponentModel.DataAnnotations;

namespace CNSVM.Models
{
    public class MedicalGroupAudit
    {
        [Key]
        public int Id { get; set; }

        public string Action { get; set; }
        public DateTime ActionDate { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }

        // Relaciones
        public User? User { get; set; }
        public MedicalGroup? MedicalGroup { get; set; }
    }

}
