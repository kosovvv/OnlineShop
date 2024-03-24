using Skinet.Core.Entities.OrderAggregate;

namespace Skinet.Core.Specification
{
    public class OrderWithItemsAndOrderingSpecification : BaseSpecification<Order>
    {
        public OrderWithItemsAndOrderingSpecification(string email) : base(x => x.BuyerEmail == email)
        {
            AddInclude(x => x.OrderItems);
            AddInclude(x => x.DeliveryMethod);
            AddOrderByDescending(x => x.OrderDate);
        }

        public OrderWithItemsAndOrderingSpecification(int id, string buyer) 
            : base(x => x.Id == id && x.BuyerEmail == buyer)
        {
            AddInclude(x => x.OrderItems);
            AddInclude(x => x.DeliveryMethod);
        }
    }
}
 