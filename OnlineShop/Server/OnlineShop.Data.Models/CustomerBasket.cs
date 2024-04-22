using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static OnlineShop.Common.Entities.CustomerBasketConstants;

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

        [Required(ErrorMessage = ItemsRequiredMessage)]
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

        public int? DeliveryMethodId { get; set; }

        public string ClientSecret { get; set; }
        public string PaymentIntentId { get; set; }

        [Required(ErrorMessage = ShippingPriceRequiredMessage)]
        [Range(0, double.MaxValue, ErrorMessage = ShippingPriceRangeMessage)]
        public decimal ShippingPrice { get; set; }
    }
}
