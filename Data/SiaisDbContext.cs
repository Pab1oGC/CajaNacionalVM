using Microsoft.EntityFrameworkCore;

namespace VeriMedCNS.Data
{
    public class SiaisDbContext : DbContext
    {
        public SiaisDbContext(DbContextOptions<SiaisDbContext> options) : base(options) { }

        // Define tus DbSet para las tablas en la base de datos siais
    }
}
