using CNSVM.Models;
using Microsoft.EntityFrameworkCore;

namespace CNSVM.Data
{
    public class CnsvmDbContext : DbContext
    {
        public CnsvmDbContext(DbContextOptions<CnsvmDbContext> options) : base(options) { }

        public DbSet<MedicalCriterion> MedicalCriterion { get; set; }
        public DbSet<Medicament> Medicament { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<MedicamentPrescription> MedicamentPrescription { get; set; }
        public DbSet<Patient> Patient { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de DoctorGroup

            // Configuración de MedicalCriterion
            modelBuilder.Entity<MedicalCriterion>(entity =>
            {
                entity.HasOne(mc => mc.User)
                      .WithMany(u => u.MedicalCriteria)
                      .HasForeignKey(mc => mc.UserId)
                      .OnDelete(DeleteBehavior.Restrict); // Cambiado a Restrict o NoAction

                entity.HasOne(mc => mc.MedicamentPrescription)
                      .WithMany(mp => mp.MedicalCriterion)
                      .HasForeignKey(mc => mc.MedicamentPrescriptionId);
            });

            // Configuración de MedicalGroupAudit

            // Configuración de MedicamentPrescription
            modelBuilder.Entity<MedicamentPrescription>(entity =>
            {
                entity.HasOne(mp => mp.Medicament)
                      .WithMany(m => m.MedicamentPrescriptions)
                      .HasForeignKey(mp => mp.MedicamentId);
            });

            // Configuración de Prescription

            base.OnModelCreating(modelBuilder);
        }

    }
}
