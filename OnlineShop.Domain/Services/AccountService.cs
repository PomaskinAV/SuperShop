using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Domain.Services;

public partial class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IApplicationPasswordHasher _hasher;

    public AccountService(IAccountRepository accountRepository, IApplicationPasswordHasher hasher)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
    }

    public async Task<Account> Login(string email, string password, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(password);

        var account = await _accountRepository.FindAccountByEmail(email, cancellationToken);
        if(account is null)
        {
            throw new AccountNotFoundException("Account with given email not found");
        }

        var isPasswordValid = _hasher.VerifyHashedPassword(account.HashedPassword, password, out var rehashNeeded);
        if(isPasswordValid)
        {
            throw new InvalidPasswordException("Invalid password");
        }

        if(rehashNeeded)
        {
            await RehashPassword(password, account, cancellationToken);
        }

        return account;
    }

    private Task RehashPassword(string password, Account account, CancellationToken cancellationToken)
    {
        account.HashedPassword = EncryptPassword(password);
        return _accountRepository.Update(account, cancellationToken);
    }

    public virtual async Task<Account> Register(string name, string email, string password, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(password);

        var existedAccount = await _accountRepository.FindAccountByEmail(email, cancellationToken);
        if (existedAccount is not null)
        {
            throw new EmailAlreadyExistsException("Account whit given email is already exist");
        }

        var account = new Account(name, email, EncryptPassword(password));
        await _accountRepository.Add(account, cancellationToken);
        return account;
    }
    private string EncryptPassword(string password)
    {
        var hashedPassword = _hasher.HashPassword(password);
        return hashedPassword;
    }
}
