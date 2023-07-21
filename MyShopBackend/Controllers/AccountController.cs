using Microsoft.AspNetCore.Mvc;
using MyShopBackend.Data;
using MyShopBackend.Data.Repositories;

namespace MyShopBackend.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("register")]
        public async Task <IActionResult> Register(
            Account account,
            IAccountRepository accountRepository,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(accountRepository);
            await accountRepository.Add(account, cancellationToken);
            return Ok();
        }
    }
}
