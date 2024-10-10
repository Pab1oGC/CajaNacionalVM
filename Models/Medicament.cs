using System.ComponentModel.DataAnnotations;

namespace CNSVM.Models
{
    public class Medicament
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string PharmaceuticalForm { get; set; }
        public string Concentration { get; set; }
        public string Classific { get; set; }
        public char RestrictedUse { get; set; }
    }
}
