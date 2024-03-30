using Microsoft.AspNetCore.Http;

namespace OnlineShop.Web.ViewModels.Image
{
    public class ImageUploadDto
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
    }
}
