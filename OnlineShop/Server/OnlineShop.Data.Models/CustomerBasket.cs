using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class CustomerBasket
    {
        public CustomerBasket()
        {

        }
        public CustomerBasket(string id)
        {
            this.Id = id;
        }

        public string Id { get; set; }

        [Required(ErrorMessage = "Items are required")]
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

        public int? DeliveryMethodId { get; set; }

        public string ClientSecret { get; set; }
        public string PaymentIntentId { get; set; }

        [Required(ErrorMessage = "Shipping price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Shipping price must be greater than or equal to 0")]
        public decimal ShippingPrice { get; set; }
    }
}
