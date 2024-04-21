using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "Product item ID is required")]
        public int ProductItemId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Picture URL is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string PictureUrl { get; set; }
    }
}
