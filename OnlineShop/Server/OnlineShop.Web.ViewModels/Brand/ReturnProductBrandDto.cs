namespace OnlineShop.Web.ViewModels.Brand
{
    public class ReturnProductBrandDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public ICollection<ProductToReturnDto> Products { get; set; } = new List<ProductToReturnDto>();
    }
}
