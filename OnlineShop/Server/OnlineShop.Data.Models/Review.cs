using OnlineShop.Data.Models.Identity;
using OnlineShop.Models;

namespace OnlineShop.Data.Models
{
    public class Review : BaseEntity
    {
        public ApplicationUser Author { get; set; }
        public Product ReviewedProduct { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsVerified { get; set; }

    }
}
