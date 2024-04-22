using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static OnlineShop.Common.Entities.ProductConstants;
using OnlineShop.Data.Common.Models;
using OnlineShop.Data.Models;

namespace OnlineShop.Models
{
    public class Product : BaseDeletableModel<int>
    {
        [Required(ErrorMessage = NameRequiredMessage)]
        public string Name { get; set; }

        [Required(ErrorMessage = DescriptionRequiredMessage)]
        public string Description { get; set; }

        [Required(ErrorMessage = PriceRequiredMessage)]
        [Range(MinPrice, MaxPrice, ErrorMessage = PriceRangeMessage)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = PictureUrlRequiredMessage)]
        [Url(ErrorMessage = PictureUrlInvalidFormatMessage)]
        public string PictureUrl { get; set; }

        [Required(ErrorMessage = ProductTypeIdRequiredMessage)]
        public int ProductTypeId { get; set; }

        public ProductType ProductType { get; set; }

        [Required(ErrorMessage = ProductBrandIdRequiredMessage)]
        public int ProductBrandId { get; set; }

        public ProductBrand ProductBrand { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        [NotMapped]
        public double AverageScore => this.Reviews.Count > 0 ? this.Reviews.Average(x => x.Score) : 0;
    }
}
