using System.ComponentModel.DataAnnotations;

namespace VeriMedCNS.Models
{
    public class MedicationRequest
    {
        [Key]
        public int RequestId { get; set; }
        public string MedicationName { get; set; }
        public int PatientId { get; set; }
        public DateTime RequestDate { get; set; }
        public char Status {  get; set; }
    }
}
