using MediatR;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Events;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using System.CodeDom.Compiler;
using System.Security.Principal;

namespace OnlineShop.Domain.Services
{
    public partial class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IApplicationPasswordHasher _hasher;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _uow;
        private readonly IEmailSender _emailSender;

        public AccountService(IApplicationPasswordHasher hasher, IUnitOfWork uow, IMediator mediator)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public virtual async Task<(Account account, Guid codeId)> Login(string email, string password, CancellationToken cancellationToken)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));
            if (password == null) throw new ArgumentNullException(nameof(password));

            var account = await LoginByPassword(email, password, cancellationToken);

            var code = await CreateAndSendConfirmationCode(account, cancellationToken);

            return (account, code.Id);
        }

        private async Task<ConfirmationCode> CreateAndSendConfirmationCode(Account account, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(account);
            var code = GenerateNewConfirmationCode(account);
            _uow.ConfirmationCodeRepository.Add(code, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            await _emailSender.SendEmailAsync(account.Email, "Подтверждение входа", $"Код подтверждения: {code.Code}", cancellationToken);
            return code;
        }

        private ConfirmationCode GenerateNewConfirmationCode(Account account)
        {
            return new ConfirmationCode(Guid.NewGuid(), account.Id, DateTimeOffset.Now, TimeSpan.Zero);
        }

        private async Task<Account?> LoginByPassword(string email, string password, CancellationToken cancellationToken)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));
            if (password == null) throw new ArgumentNullException(nameof(password));

            var account = await _accountRepository.FindAccountByEmail(email, cancellationToken);
            if (account is null)
            {
                throw new AccountNotFoundException("Account with given email not found");
            }

            var isPasswordValid = _hasher.VerifyHashedPassword(account.HashedPassword, password, out var rehashNeeded);
            if (isPasswordValid)
            {
                throw new InvalidPasswordException("Invalid password");
            }

            if (rehashNeeded)
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
            await _mediator.Publish(new AccountRegistered(account, DateTime.UtcNow), cancellationToken);
            return account;
        }
        private string EncryptPassword(string password)
        {
            var hashedPassword = _hasher.HashPassword(password);
            return hashedPassword;
        }

        public async Task<Account> LoginByCode(string email, Guid codeId, string code, CancellationToken cancellationToken)
        {
            var codeObject = await _uow.ConfirmationCodeRepository.GetById(codeId, cancellationToken);
            if (codeObject is null)
            {
                throw new CodeNotFoundException("There is no Code for this CodeId");
            }
            if (codeObject.Code != code)
            {
                throw new InvalidCodeException("Code not confirmed!");
            }
            var account = await _uow.AccountRepository.FindAccountByEmail(email, cancellationToken);
            if (account is null)
                throw new AccountNotFoundException("Account not found");
            return account;
        }
    }
}
