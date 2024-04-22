using System.ComponentModel.DataAnnotations;
using static OnlineShop.Common.Entities.DeliveryMethodConstants;
using OnlineShop.Data.Common.Models;

namespace OnlineShop.Data.Models.OrderAggregate
{
    public class DeliveryMethod : BaseModel<int>
    {
        [Required(ErrorMessage = ShortNameRequiredMessage)]
        public string ShortName { get; set; }

        [Required(ErrorMessage = DeliveryTimeRequiredMessage)]
        public string DeliveryTime { get; set; }

        [Required(ErrorMessage = DescriptionRequiredMessage)]
        public string Description { get; set; }

        [Required(ErrorMessage = PriceRequiredMessage)]
        [Range(0, double.MaxValue, ErrorMessage = PriceRangeMessage)]
        public decimal Price { get; set; }
    }
}
