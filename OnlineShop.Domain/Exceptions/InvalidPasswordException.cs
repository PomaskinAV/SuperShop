using OnlineShop.Domain.Exceptions;

namespace OnlineShop.Domain.Services;

public partial class AccountService
{
    public class InvalidPasswordException : DomainException
    {
        public InvalidPasswordException(string message) : base(message)
        {
        }
    }
}
