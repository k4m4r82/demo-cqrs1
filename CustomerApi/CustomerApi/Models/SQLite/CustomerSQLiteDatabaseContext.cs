using Microsoft.EntityFrameworkCore;

namespace CustomerApi.Models.SQLite
{
	public class CustomerSQLiteDatabaseContext : DbContext
	{
		public CustomerSQLiteDatabaseContext(DbContextOptions<CustomerSQLiteDatabaseContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CustomerRecord>()
						.HasMany(x => x.Phones)
						.WithOne(x => x.Customer)
						.HasForeignKey(x => x.CustomerId);
		}

		public DbSet<CustomerRecord> Customers { get; set; }
	}
}
