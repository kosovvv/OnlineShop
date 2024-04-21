using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Required(ErrorMessage = "Order date is required")]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Ship-to address is required")]
        public OrderAddress ShipToAddress { get; set; }

        [Required(ErrorMessage = "Delivery method ID is required")]
        public int DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }

        [Required(ErrorMessage = "Order items are required")]
        public ICollection<OrderItem> OrderItems { get; set; }

        [Required(ErrorMessage = "Subtotal is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Subtotal must be greater than or equal to 0")]
        public decimal SubTotal { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public string PaymentIntentId { get; set; }

        [NotMapped]
        public decimal GetTotal => SubTotal + DeliveryMethod.Price;
    }
}
