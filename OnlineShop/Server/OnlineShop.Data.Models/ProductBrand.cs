using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OnlineShop.Data.Common.Models;

namespace OnlineShop.Models
{
    public class ProductBrand : BaseDeletableModel<int>
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Url(ErrorMessage = "Invalid URL format")]
        public string PictureUrl { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
