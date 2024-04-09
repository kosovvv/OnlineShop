using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Web.ViewModels
{
    public class CustomerBasketDto
    {

        [Required]
        public string Id { get; set; }
        public ICollection<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
        public int? DeliveryMethodId { get; set; }
        public string ClientSecret { get; set; }
        public string PaymentIntentId { get; set; }
        public decimal ShippingPrice { get; set; }
    }
}
