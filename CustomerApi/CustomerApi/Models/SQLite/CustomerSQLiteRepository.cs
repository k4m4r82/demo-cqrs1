namespace CustomerApi.Models.SQLite
{
	public class CustomerSQLiteRepository
	{
		private readonly CustomerSQLiteDatabaseContext _context;

		public CustomerSQLiteRepository(CustomerSQLiteDatabaseContext context)
		{
			_context = context;
		}

		public CustomerRecord Create(CustomerRecord customer)
		{
			Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<CustomerRecord> entry = _context.Customers.Add(customer);
			_context.SaveChanges();
			return entry.Entity;
		}

		public void Update(CustomerRecord customer)
		{
			_context.SaveChanges();
		}

		public void Remove(long id)
		{
			_context.Customers.Remove(GetById(id));
			_context.SaveChanges();
		}

		public CustomerRecord GetById(long id)
		{
			return _context.Customers.Find(id);
		}
	}
}
