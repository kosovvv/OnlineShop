using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Web.ViewModels.Brand
{
    public class CreateProductBrandDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Picture URL is required")]
        public string PictureUrl { get; set; }
    }
}
