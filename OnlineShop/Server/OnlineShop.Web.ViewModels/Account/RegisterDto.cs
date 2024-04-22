using OnlineShop.Common.Entities;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Web.ViewModels
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(UserConstants.PasswordRegex,
            ErrorMessage = "Password must have 1 uppercase, 1 lowercase, 1 number and 1 non alpha-numeric and" +
            "at least 6 characters")]
        public string Password { get; set; }
    }
}
