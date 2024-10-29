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
        public DbSet<MedicalGroup> MedicalGroup { get; set; }
        public DbSet<MedicalGroupAudit> MedicalGroupAudit { get; set; }
		public DbSet<Medicament> Medicament { get; set; }
		public DbSet<User> User { get; set; }

        public DbSet<MedicamentPrescription> MedicamentPrescription { get; set; }
       
    }

}
