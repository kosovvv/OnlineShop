using System.ComponentModel.DataAnnotations;
using static OnlineShop.Common.Entities.ProductBrandConstants;
using OnlineShop.Data.Common.Models;

namespace OnlineShop.Models
{
    public class ProductBrand : BaseDeletableModel<int>
    {
        [Required(ErrorMessage = NameRequiredMessage)]
        public string Name { get; set; }

        [Url(ErrorMessage = PictureUrlInvalidFormatMessage)]
        public string PictureUrl { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
