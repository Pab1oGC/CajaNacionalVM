using Microsoft.EntityFrameworkCore;

namespace CNSVM.Data
{
	public class SiaisDbContext : DbContext
	{
		public SiaisDbContext(DbContextOptions<SiaisDbContext> options) : base(options) { }

		// Define tus DbSet para las tablas en la base de datos siais
	}
}
