using System.ComponentModel.DataAnnotations;
using static OnlineShop.Common.Entities.OrderItemConstants;

namespace OnlineShop.Web.ViewModels
{
    public class OrderItemDto
    {
        [Required(ErrorMessage = ProductIdRequiredMessage)]
        public int ProductId { get; set; }

        [Required(ErrorMessage = ProductNameRequiredMessage)]
        public string ProductName { get; set; }

        [Required(ErrorMessage = PictureUrlRequiredMessage)]
        [Url(ErrorMessage = PictureUrlInvalidFormatMessage)]
        public string PictureUrl { get; set; }

        [Required(ErrorMessage = PriceRequiredMessage)]
        [Range(0, double.MaxValue, ErrorMessage = PriceRangeMessage)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = QuantityRequiredMessage)]
        [Range(1, int.MaxValue, ErrorMessage = QuantityRangeMessage)]
        public int Quantity { get; set; }
    }
}
