using System.ComponentModel.DataAnnotations;
using static OnlineShop.Common.Entities.ProductConstants;

namespace OnlineShop.Web.ViewModels.Product
{
    public class ProductToCreateDto
    {
        [Required(ErrorMessage = NameRequiredMessage)]
        public string Name { get; set; }

        [Required(ErrorMessage = DescriptionRequiredMessage)]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = PriceRequiredMessage)]
        [Range(0, double.MaxValue, ErrorMessage = PriceRangeMessage)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = PictureUrlRequiredMessage)]
        public string PictureUrl { get; set; }

        [Required(ErrorMessage = ProductTypeIdRequiredMessage)]
        public string ProductType { get; set; }

        [Required(ErrorMessage = ProductBrandIdRequiredMessage)]
        public string ProductBrand { get; set; }
    }
}
