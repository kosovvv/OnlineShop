using Skinet.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet.Core.Entities.OrderAggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {
            
        }
        public Order(ICollection<OrderItem> orderItems, string buyerEmail, OrderAddress shipToAddress, DeliveryMethod deliveryMethod, decimal subTotal)
        {
            BuyerEmail = buyerEmail;    
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            SubTotal = subTotal;
        }

        public string BuyerEmail { get; set; }
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
