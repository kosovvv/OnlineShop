using OnlineShop.Data.Common.Models;
using OnlineShop.Data.Models.Identity;
using OnlineShop.Models;

namespace OnlineShop.Data.Models
{
    public class Review : BaseModel<int>
    {
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public int Score { get; set; }
        public Product ReviewedProduct { get; set; }
        public string Description { get; set; }
        public bool IsVerified { get; set; }

    }
}
