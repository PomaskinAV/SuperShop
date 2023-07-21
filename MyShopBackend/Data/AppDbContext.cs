using Microsoft.EntityFrameworkCore;

namespace MyShopBackend.Data
{
	public class AppDbContext: DbContext
	{
			//Список таблиц:
			public DbSet<Product> Products => Set<Product>();
        public DbSet<Account> Accounts => Set<Account>();

        public AppDbContext(
				DbContextOptions<AppDbContext> options)
				: base(options)
			{
			}
		}
}
