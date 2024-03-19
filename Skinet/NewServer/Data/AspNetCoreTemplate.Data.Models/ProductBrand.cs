namespace OnlineShop.Data.Models;

using AspNetCoreTemplate.Data.Common.Models;

public class ProductBrand : BaseDeletableModel<int>
{
    public string Name { get; set; }
}
