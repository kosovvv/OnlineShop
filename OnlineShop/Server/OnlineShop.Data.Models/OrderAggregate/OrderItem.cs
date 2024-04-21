using System.ComponentModel.DataAnnotations;
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

        [Required(ErrorMessage = "Item ordered is required")]
        public ProductItemOrdered ItemOrdered { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}
