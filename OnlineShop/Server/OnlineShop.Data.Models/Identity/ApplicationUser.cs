using Microsoft.AspNetCore.Identity;
using OnlineShop.Data.Models.OrderAggregate;

namespace OnlineShop.Data.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
} 
