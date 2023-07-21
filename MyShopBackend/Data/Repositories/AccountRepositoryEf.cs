using Microsoft.EntityFrameworkCore;

namespace MyShopBackend.Data.Repositories
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
    }
}
