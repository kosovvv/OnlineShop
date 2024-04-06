using OnlineShop.Data.Models.Identity;
using OnlineShop.Models;

namespace OnlineShop.Data.Models
{
    public class Rating : BaseEntity
    {
        public Product RatedProduct { get; set; }
        public int Score { get; set; }
        public ApplicationUser Author { get; set; }
    }
}
