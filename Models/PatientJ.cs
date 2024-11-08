namespace CNSVM.Models
{
    public class PatientJ
    {
        public int Matricula { get; set; }
        public int CarnetIdentidad { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefonos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public List<ClinicalHistoryJ> historias_clinicas { get; set; }
        

        // Nueva propiedad calculada para la edad
        public int Edad => DateTime.Now.Year - FechaNacimiento.Year - (DateTime.Now.DayOfYear < FechaNacimiento.DayOfYear ? 1 : 0);

    }
}
