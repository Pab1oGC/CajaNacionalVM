using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace CNSVM.Models
{
    public class ClinicalHistory
    {
        [Key]
        public int idHistoria { get; set; }
        public DateTime fecha { get; set; }
        public string diagnostico { get; set; }
        public string antecedentes { get; set; }
        public int medicoId { get; set; }
        public List<MedicationJ> medicamentos { get; set; }
    }
}
