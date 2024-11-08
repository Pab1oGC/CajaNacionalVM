using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace CNSVM.Models
{
    public class ClinicalHistory
    {
        [Key]
        public int IdHistoria { get; set; }
        public DateTime Fecha { get; set; }
        public string? Diagnostico { get; set; }
        public string? Antecedentes { get; set; }
        public List<Medicament> Medicamentos { get; set; } = new List<Medicament>();
    }
}
