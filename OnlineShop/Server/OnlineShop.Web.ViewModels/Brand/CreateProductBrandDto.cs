using System.ComponentModel.DataAnnotations;
using static OnlineShop.Common.Entities.ProductBrandConstants;

namespace OnlineShop.Web.ViewModels.Brand
{
    public class CreateProductBrandDto
    {
        [Required(ErrorMessage = NameRequiredMessage)]
        public string Name { get; set; }

        [Required(ErrorMessage = PictureUrlInvalidFormatMessage)]
        public string PictureUrl { get; set; }
    }
}
