using OnlineShop.Data.Models.Identity;
using OnlineShop.Models;

namespace OnlineShop.Data.Models
{
    public class Review : BaseEntity
    {
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public int Score { get; set; }
        public Product ReviewedProduct { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsVerified { get; set; }

    }
}
