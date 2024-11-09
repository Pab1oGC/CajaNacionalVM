namespace CNSVM.Models
{
    public class ClinicalHistoryJ
    {
        public int IdHistoria { get; set; }
        public DateTime Fecha { get; set; }
        public string Diagnostico { get; set; }
        public string Antecedentes { get; set; }
        public int medicoId { get; set; }
        public List<MedicationJ> Medicamentos { get; set; }
    }

}
