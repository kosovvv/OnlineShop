using System.ComponentModel.DataAnnotations;
using static OnlineShop.Common.Entities.ProductTypeConstants;
namespace OnlineShop.Web.ViewModels.Type
{
    public class CreateProductTypeDto
    {
        [Required(ErrorMessage = NameRequiredMessage)]
        public string Name { get; set; }

        [Required(ErrorMessage = PictureUrlInvalidFormatMessage)]
        public string PictureUrl { get; set; }
    }
}
