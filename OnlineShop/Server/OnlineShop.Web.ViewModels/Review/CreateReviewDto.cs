using OnlineShop.Data.Models.Identity;

namespace OnlineShop.Web.ViewModels.Review
{
    public class CreateReviewDto
    {
        public int Score { get; set; }
        public ProductToReturnDto ReviewedProduct { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public bool IsVerified { get; set; }
    }
}
