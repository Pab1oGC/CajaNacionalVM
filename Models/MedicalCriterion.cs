﻿using System.ComponentModel.DataAnnotations;

namespace CNSVM.Models
{
    public class MedicalCriterion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public char Criterion { get; set; }

        public DateTime CriterionDate { get; set; }
        public string? CriterionReason { get; set; }
        public int UserId { get; set; }
        public int MedicamentPrescriptionId { get; set; }

        // Relaciones
        public User User { get; set; }
        public MedicamentPrescription MedicamentPrescription { get; set; }
    }
}
