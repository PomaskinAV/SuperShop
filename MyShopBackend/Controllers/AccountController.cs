using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Services;
using OnlineShop.HttpModel.Requests;
using OnlineShop.HttpModels.Responses;
using System.Security.Claims;
using static OnlineShop.Domain.Services.AccountService;

namespace MyShopBackend.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(AccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }
        [HttpPost("register")]
        public async Task <IActionResult> Register(
            RegisterRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                await _accountService.Register(request.Name, request.Email, request.Password, cancellationToken);
            }catch (EmailAlreadyExistsException)
            {
                return Conflict(new ErrorResponse("Такой аккаунт уже зарегистрирован!"));

            }
            
            return Ok();
        }

        [HttpPost("login_by_password")]
        public async Task<ActionResult<LoginByPasswordResponse>> LoginByPassword(LoginRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var (account, codeId) = await _accountService.Login(request.Email, request.Password, cancellationToken);
                var token = _tokenService.GenerateToken(account);
                return new LoginByPasswordResponse(account.Id, account.Name, codeId);
            }catch(AccountNotFoundException)
            {
                return Conflict(new ErrorResponse("Аккаунт с таким Email не найден!"));
            }catch(InvalidPasswordException)
            {
                return Conflict(new ErrorResponse("Неверный пароль!"));
            }
        }

        [HttpPost("login_by_code")]

        public async Task<ActionResult<LoginByCodeResponse>> LoginByCode(
            LoginByCodeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _accountService.LoginByCode(request.Email, request.CodeId, request.Code, cancellationToken);
                var token = _tokenService.GenerateToken(account);
                return new LoginByCodeResponse(account.Id, account.Name, token);
            }
            catch (AccountNotFoundException)
            {
                return Conflict(new ErrorResponse("Аккаунт с таким Email не найден!"));
            }
            catch (InvalidCodeException)
            {
                return Conflict(new ErrorResponse("Неверный код"));
            }
        }
    }
}
