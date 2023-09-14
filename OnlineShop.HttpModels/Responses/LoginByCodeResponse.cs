namespace MyShopBackend.Controllers
{
    public record LoginByCodeResponse (Guid Id, string Name, string Token);
}
