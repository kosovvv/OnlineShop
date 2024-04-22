using System.ComponentModel.DataAnnotations;
using static OnlineShop.Common.Entities.ReviewConstants;
using OnlineShop.Data.Common.Models;
using OnlineShop.Data.Models.Identity;
using OnlineShop.Models;

namespace OnlineShop.Data.Models
{
    public class Review : BaseModel<int>
    {
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        [Required(ErrorMessage = ScoreRequiredMessage)]
        [Range(MinScore, MaxScore, ErrorMessage = ScoreRangeMessage)]
        public int Score { get; set; }

        [Required(ErrorMessage = ReviewedProductIdRequiredMessage)]
        public int ReviewedProductId { get; set; }
        public Product ReviewedProduct { get; set; }

        [Required(ErrorMessage = DescriptionRequiredMessage)]
        [MaxLength(MaxDescriptionLength, ErrorMessage = DescriptionMaxLengthMessage)]
        public string Description { get; set; }

        public bool IsVerified { get; set; }
    }
}
