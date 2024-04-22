using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static OnlineShop.Common.Entities.OrderConstants;
using OnlineShop.Data.Common.Models;
using OnlineShop.Data.Models.Enumerations;
using OnlineShop.Data.Models.Identity;

namespace OnlineShop.Data.Models.OrderAggregate
{
    public class Order : BaseModel<int>
    {
        public Order()
        {

        }

        public Order(ICollection<OrderItem> orderItems, string buyerId, OrderAddress shipToAddress, DeliveryMethod deliveryMethod, int deliveryMethodId, decimal subTotal, string paymentIntentId)
        {
            BuyerId = buyerId;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            DeliveryMethodId = deliveryMethodId;
            OrderItems = orderItems;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerId { get; set; }
        public ApplicationUser Buyer { get; set; }

        [Required(ErrorMessage = OrderDateRequiredMessage)]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = ShipToAddressRequiredMessage)]
        public OrderAddress ShipToAddress { get; set; }

        [Required(ErrorMessage = DeliveryMethodIdRequiredMessage)]
        public int DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }

        [Required(ErrorMessage = OrderItemsRequiredMessage)]
        public ICollection<OrderItem> OrderItems { get; set; }

        [Required(ErrorMessage = SubTotalRequiredMessage)]
        [Range(0, double.MaxValue, ErrorMessage = SubTotalRangeMessage)]
        public decimal SubTotal { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public string PaymentIntentId { get; set; }

        [NotMapped]
        public decimal GetTotal => SubTotal + DeliveryMethod.Price;
    }
}
