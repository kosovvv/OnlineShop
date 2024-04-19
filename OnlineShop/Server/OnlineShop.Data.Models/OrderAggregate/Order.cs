using OnlineShop.Data.Common.Models;
using OnlineShop.Data.Models.Enumerations;
using OnlineShop.Data.Models.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Data.Models.OrderAggregate
{
    public class Order : BaseModel<int>
    {
        public Order()
        {
            
        }
        public Order(ICollection<OrderItem> orderItems, string buyerId, OrderAddress shipToAddress, DeliveryMethod deliveryMethod, decimal subTotal, string paymentIntentId)
        {
            BuyerId = buyerId;    
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerId { get; set; }
        public ApplicationUser Buyer { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public OrderAddress ShipToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public decimal SubTotal { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public string PaymentIntentId { get; set; }

        [NotMapped]
        public decimal GetTotal => SubTotal + DeliveryMethod.Price;


    }
}
