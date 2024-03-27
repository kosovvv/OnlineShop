using Skinet.Core.Entities.OrderAggregate;

namespace Skinet.Core.Specification
{
    public class OrderByPaymentIntentIdSpecification : BaseSpecification<Order>
    {
        public OrderByPaymentIntentIdSpecification(string paymentIntentId) 
            : base(x => x.PaymentIntentId == paymentIntentId)
        {
            
        }
    }
}
