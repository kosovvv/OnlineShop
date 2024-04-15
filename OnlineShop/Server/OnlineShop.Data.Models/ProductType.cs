namespace OnlineShop.Models
{
    public class ProductType : BaseEntity
    {
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public ICollection<Product> Products { get; set;} = new List<Product>();
    }
}