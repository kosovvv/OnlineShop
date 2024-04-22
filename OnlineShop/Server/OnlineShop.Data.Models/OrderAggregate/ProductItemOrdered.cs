using System.ComponentModel.DataAnnotations;
using static OnlineShop.Common.Entities.ProductItemOrderedConstants;

namespace OnlineShop.Data.Models.OrderAggregate
{
    public class ProductItemOrdered
    {
        public ProductItemOrdered()
        {

        }

        public ProductItemOrdered(int productItemId, string productName, string pictureUrl)
        {
            ProductItemId = productItemId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }

        [Required(ErrorMessage = ProductItemIdRequiredMessage)]
        public int ProductItemId { get; set; }

        [Required(ErrorMessage = ProductNameRequiredMessage)]
        public string ProductName { get; set; }

        [Required(ErrorMessage = PictureUrlRequiredMessage)]
        [Url(ErrorMessage = PictureUrlInvalidFormatMessage)]
        public string PictureUrl { get; set; }
    }
}
