using CNSVM.Models;
using Microsoft.EntityFrameworkCore;

namespace CNSVM.Data
{
    public class CnsvmDbContext : DbContext
    {
        public CnsvmDbContext(DbContextOptions<CnsvmDbContext> options) : base(options) { }

        public DbSet<Prescription> Prescription { get; set; }
        public DbSet<DoctorGroup> DoctorGroup { get; set; }
        public DbSet<MedicalCriterion> MedicalCriterion { get; set; }
        public DbSet<MedicalGroupAudit> MedicalGroupAudit { get; set; }
        public DbSet<MedicalGroup> MedicalGroup { get; set; }
        public DbSet<Medicament> Medicament { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<MedicamentPrescription> MedicamentPrescription { get; set; }
        public DbSet<Patient> Patient { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de DoctorGroup
            modelBuilder.Entity<DoctorGroup>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.GroupId });

                entity.HasOne(dg => dg.User)
                      .WithMany(u => u.DoctorGroups)
                      .HasForeignKey(dg => dg.UserId);

                entity.HasOne(dg => dg.MedicalGroup)
                      .WithMany(mg => mg.DoctorGroups)
                      .HasForeignKey(dg => dg.GroupId);
            });

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
            modelBuilder.Entity<MedicalGroupAudit>(entity =>
            {
                entity.HasOne(mga => mga.User)
                      .WithMany(u => u.MedicalGroupAudits)
                      .HasForeignKey(mga => mga.UserId);

                entity.HasOne(mga => mga.MedicalGroup)
                      .WithMany(mg => mg.MedicalGroupAudits)
                      .HasForeignKey(mga => mga.GroupId);
            });

            // Configuración de MedicamentPrescription
            modelBuilder.Entity<MedicamentPrescription>(entity =>
            {
                entity.HasOne(mp => mp.Prescription)
                      .WithMany(p => p.MedicamentPrescriptions)
                      .HasForeignKey(mp => mp.PrescriptionId);

                entity.HasOne(mp => mp.Medicament)
                      .WithMany(m => m.MedicamentPrescriptions)
                      .HasForeignKey(mp => mp.MedicamentId);
            });

            // Configuración de Prescription
            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasOne(p => p.Doctor)
                      .WithMany(u => u.Prescriptions)
                      .HasForeignKey(p => p.DoctorId);
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
