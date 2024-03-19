namespace OnlineShop.Data.Models;

using AspNetCoreTemplate.Data.Common.Models;

public class ProductType : BaseDeletableModel<int>
{
    public string Name { get; set; }
}
