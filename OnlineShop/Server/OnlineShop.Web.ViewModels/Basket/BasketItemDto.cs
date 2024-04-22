using System.ComponentModel.DataAnnotations;
using static OnlineShop.Common.Entities.BasketConstants;

namespace OnlineShop.Web.ViewModels
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        [Range(MinPrice, double.MaxValue, ErrorMessage = PriceRangeMessage)]
        public decimal Price { get; set; }

        [Required]
        [Range(MinQuantity, double.MaxValue, ErrorMessage = QuantityRangeMessage)]
        public int Quantity { get; set; }

        [Required]
        public string PictureUrl { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
