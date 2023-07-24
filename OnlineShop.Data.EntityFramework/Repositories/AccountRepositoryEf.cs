using Microsoft.EntityFrameworkCore;
using OnlineShop.Data.EntityFramework;
using OnlineShop.Domain;
using OnlineShop.Domain.Interfaces;

namespace MyShopBackend.Repositories
{
    public class AccountRepositoryEf : EFRepository<Account>, IAccountRepository
    {
        public AccountRepositoryEf(AppDbContext dbContext) : base(dbContext)
        {
        }

        public Task<Account> GetAccountByEmail(string email, CancellationToken cancellationToken)
        {
            if (email is null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            return Entities.SingleAsync(e => e.Email == email, cancellationToken);
        }
        public Task<Account?> FindAccountByEmail(string email, CancellationToken cancellationToken)
        {
            if (email is null)
            {
                throw new ArgumentNullException(nameof(email));
            }
            return Entities.SingleOrDefaultAsync(e => e.Email == email, cancellationToken);
        }
    }
}
