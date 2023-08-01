using System.ComponentModel.DataAnnotations;

namespace OnlineShop.HttpModel.Requests
{
    public class LoginRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(30, ErrorMessage ="Пароль минимум 8 символов", MinimumLength = 8)]
        public string Password { get; set; }
    }
}
