using CNSVM.Models;
using Microsoft.EntityFrameworkCore;

namespace CNSVM.Data
{
	public class ErpcnsDbContext : DbContext
	{
		public ErpcnsDbContext(DbContextOptions<ErpcnsDbContext> options) : base(options)
		{

		}
		public DbSet<Patient> Patient { get; set; }
	}
}
