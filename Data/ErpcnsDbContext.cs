using Microsoft.EntityFrameworkCore;
using VeriMedCNS.Models;

namespace VeriMedCNS.Data
{
    public class ErpcnsDbContext: DbContext
    {
        public ErpcnsDbContext(DbContextOptions<ErpcnsDbContext> options):base(options)
        {

        }
        public DbSet<Patient> Patient { get; set; }
    }
}
