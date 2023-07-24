using System.ComponentModel.DataAnnotations;

namespace OnlineShop.HttpModel.Requests;

public class RegisterRequest
{
    [Required]
	[StringLength(20, ErrorMessage = "Имя должно содержать больше 3-х символов", MinimumLength = 3)]
	public string Name { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
	[StringLength(30, ErrorMessage = "Пароль минимум 8 символов", MinimumLength = 8)]
	public string Password { get; set; }

	[Required]
	[Compare(nameof(Password))]
	public string ConfirmedPassword { get; set; }
}
