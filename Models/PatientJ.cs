using System.ComponentModel.DataAnnotations;

namespace CNSVM.Models
{
    public class PatientJ
    {
        [Key]
        public int matricula { get; set; }
        public int carnetIdentidad { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string telefonos { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public string genero { get; set; }
        public List<ClinicalHistoryJ> historiasClinicas { get; set; }
        

        // Nueva propiedad calculada para la edad
        public int Edad => DateTime.Now.Year - fechaNacimiento.Year - (DateTime.Now.DayOfYear < fechaNacimiento.DayOfYear ? 1 : 0);

    }
}
