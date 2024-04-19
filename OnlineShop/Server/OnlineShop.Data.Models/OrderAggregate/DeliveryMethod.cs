using OnlineShop.Data.Common.Models;

namespace OnlineShop.Data.Models.OrderAggregate
{
    public class DeliveryMethod : BaseModel<int>
    {
        public string ShortName { get; set; }
        public string DeliveryTime { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
