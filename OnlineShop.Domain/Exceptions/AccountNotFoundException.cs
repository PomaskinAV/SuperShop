using OnlineShop.Domain.Exceptions;

namespace OnlineShop.Domain.Services
{
    public partial class AccountService
    {
        public class AccountNotFoundException : DomainException
        {
            public AccountNotFoundException(string message) : base(message)
            {
            }
        }
    }
}
