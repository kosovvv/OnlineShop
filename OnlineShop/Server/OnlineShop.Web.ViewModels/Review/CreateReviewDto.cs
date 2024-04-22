using System.ComponentModel.DataAnnotations;
using static OnlineShop.Common.Entities.ReviewConstants;

namespace OnlineShop.Web.ViewModels.Review
{
    public class CreateReviewDto
    {
        [Required(ErrorMessage = ScoreRequiredMessage)]
        [Range(MinScore, MaxScore, ErrorMessage = ScoreRangeMessage)]
        public int Score { get; set; }

        public ProductToReturnDto ReviewedProduct { get; set; }

        [StringLength(MaxDescriptionLength, ErrorMessage = DescriptionMaxLengthMessage)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow.ToLocalTime();

        public bool IsVerified { get; set; }
    }
}
