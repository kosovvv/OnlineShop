using Microsoft.AspNetCore.Identity;
using OnlineShop.Models;

namespace OnlineShop.Data.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }
        public ICollection<Rating> PostedRatings { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Product> FavouriteProducts { get; set; }
    }
} 
