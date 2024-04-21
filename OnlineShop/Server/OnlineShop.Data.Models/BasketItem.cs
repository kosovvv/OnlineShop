using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class BasketItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Picture URL is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string PictureUrl { get; set; }

        [Required(ErrorMessage = "Brand is required")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; }
    }
}
