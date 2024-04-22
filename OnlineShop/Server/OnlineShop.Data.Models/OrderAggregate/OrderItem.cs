using System.ComponentModel.DataAnnotations;
using static OnlineShop.Common.Entities.OrderItemConstants;
using OnlineShop.Data.Common.Models;

namespace OnlineShop.Data.Models.OrderAggregate
{
    public class OrderItem : BaseModel<int>
    {
        public OrderItem()
        {

        }

        public OrderItem(ProductItemOrdered itemOrdered, decimal price, int quantity)
        {
            ItemOrdered = itemOrdered;
            Price = price;
            Quantity = quantity;
        }

        [Required(ErrorMessage = ItemOrderedRequiredMessage)]
        public ProductItemOrdered ItemOrdered { get; set; }

        [Required(ErrorMessage = PriceRequiredMessage)]
        [Range(0, double.MaxValue, ErrorMessage = PriceRangeMessage)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = QuantityRequiredMessage)]
        [Range(1, int.MaxValue, ErrorMessage = QuantityRangeMessage)]
        public int Quantity { get; set; }
    }
}
