using OnlineShop.Web.ViewModels.Address;

namespace OnlineShop.Web.ViewModels
{
    public class OrderDto
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public ReturnAddressDto ShipToAddress { get; set; }

    }
}
