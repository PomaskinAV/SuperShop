using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Services;
using OnlineShop.HttpModel.Requests;

namespace MyShopBackend.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }
        [HttpPost("register")]
        public async Task <IActionResult> Register(
            RegisterRequest request,
            CancellationToken cancellationToken)
        {
            await _accountService.Register(request.Name, request.Email, request.Password, cancellationToken);
            return Ok();
        }
    }
}
