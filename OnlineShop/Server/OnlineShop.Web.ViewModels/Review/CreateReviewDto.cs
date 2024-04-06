using OnlineShop.Data.Models.Identity;

namespace OnlineShop.Web.ViewModels.Review
{
    public class CreateReviewDto
    {
        public ApplicationUser Author { get; set; }
        public ProductToReturnDto ReviewedProduct { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsVerified { get; set; }
    }
}
