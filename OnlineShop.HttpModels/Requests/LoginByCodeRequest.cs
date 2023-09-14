using System.ComponentModel.DataAnnotations;

namespace OnlineShop.HttpModel.Requests
{
    public class LoginByCodeRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public Guid CodeId { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
