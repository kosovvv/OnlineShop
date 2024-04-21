using OnlineShop.Data.Common.Models;
using OnlineShop.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace OnlineShop.Models
{
    public class Product : BaseDeletableModel<int>
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Picture URL is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string PictureUrl { get; set; }

        [Required(ErrorMessage = "Product type ID is required")]
        public int ProductTypeId { get; set; }

        public ProductType ProductType { get; set; }

        [Required(ErrorMessage = "Product brand ID is required")]
        public int ProductBrandId { get; set; }

        public ProductBrand ProductBrand { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        [NotMapped]
        public double AverageScore => this.Reviews.Count > 0 ? this.Reviews.Average(x => x.Score) : 0;
    }
}
