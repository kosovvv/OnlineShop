using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Web.ViewModels
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
