using System.ComponentModel.DataAnnotations;

namespace CNSVM.Models
{
    public class MedicationJ
    {
        [Key]
        public int IdMedicamento { get; set; }
        public string nombreMedicamento { get; set; }
        public string dosis { get; set; }
        public string frecuencia { get; set; }
    }

}
