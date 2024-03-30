using Microsoft.AspNetCore.Identity;

namespace OnlineShop.Data.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }
    }
} 
