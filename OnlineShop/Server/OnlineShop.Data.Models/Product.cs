using OnlineShop.Data.Models;
using OnlineShop.Data.Models.Identity;

namespace OnlineShop.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public ProductType ProductType { get; set; }
        public ProductBrand ProductBrand { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<ApplicationUser> UsersFavouringTheProduct { get; set; }

    }

}
