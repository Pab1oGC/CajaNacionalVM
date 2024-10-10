using Microsoft.EntityFrameworkCore;
using VeriMedCNS.Models;

namespace VeriMedCNS.Data
{
    public class CnsvmDbContext : DbContext
    {
        public CnsvmDbContext(DbContextOptions<CnsvmDbContext> options) : base(options) { }

        public DbSet<MedicationRequest> MedicationRequest { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Votes> Votes { get; set; }

    }

}
