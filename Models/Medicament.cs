using System.ComponentModel.DataAnnotations.Schema;

namespace CNSVM.Models
{
    public class Medicament
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? PharmaceuticalForm { get; set; }

         // Mapea la propiedad a la columna correcta en la BD
        public string? Consentration { get; set; }

          // Mapea la propiedad a la columna correcta en la BD
        public string? Clasific { get; set; }
        public char RestrictedUse { get; set; }
    }
}
