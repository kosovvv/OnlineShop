using System.ComponentModel.DataAnnotations;
using OnlineShop.Data.Common.Models;

namespace OnlineShop.Data.Models.OrderAggregate
{
    public class DeliveryMethod : BaseModel<int>
    {
        [Required(ErrorMessage = "Short name is required")]
        public string ShortName { get; set; }

        [Required(ErrorMessage = "Delivery time is required")]
        public string DeliveryTime { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal Price { get; set; }
    }
}
