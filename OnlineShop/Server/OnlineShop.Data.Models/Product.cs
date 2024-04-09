using OnlineShop.Data.Models;
using OnlineShop.Data.Models.Identity;
using System.ComponentModel.DataAnnotations.Schema;

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
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<ApplicationUser> UsersFavouringTheProduct { get; set; }

        [NotMapped]
        public double AverageScore => this.Reviews.Count > 0 ? this.Reviews.Average(x => x.Score) : 0;

    }

}
