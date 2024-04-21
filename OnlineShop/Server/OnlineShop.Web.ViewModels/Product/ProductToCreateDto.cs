using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Web.ViewModels.Product
{
    public class ProductToCreateDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Picture URL is required")]
        public string PictureUrl { get; set; }

        [Required(ErrorMessage = "Product type is required")]
        public string ProductType { get; set; }

        [Required(ErrorMessage = "Product brand is required")]
        public string ProductBrand { get; set; }
    }
}
