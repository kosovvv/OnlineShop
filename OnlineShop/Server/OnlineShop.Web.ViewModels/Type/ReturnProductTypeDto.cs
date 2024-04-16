namespace OnlineShop.Web.ViewModels.Type
{
    public class ReturnProductTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public ICollection<ProductToReturnDto> Products { get; set; } = new List<ProductToReturnDto>();
    }
}
