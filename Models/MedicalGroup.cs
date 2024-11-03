using System.ComponentModel.DataAnnotations;

namespace CNSVM.Models
{
    public class MedicalGroup
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del grupo es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre del grupo no puede exceder los 50 carácteres")]
        public string Name { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Status { get; set; }

        // Relaciones
        public ICollection<MedicalGroupAudit>? MedicalGroupAudits { get; set; }
        public ICollection<DoctorGroup>? DoctorGroups { get; set; }
    }

}
