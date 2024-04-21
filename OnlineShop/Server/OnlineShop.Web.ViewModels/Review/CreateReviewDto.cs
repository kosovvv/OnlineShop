using System;
using System.ComponentModel.DataAnnotations;
using OnlineShop.Web.ViewModels.Product;

namespace OnlineShop.Web.ViewModels.Review
{
    public class CreateReviewDto
    {
        [Required(ErrorMessage = "Score is required")]
        [Range(1, 10, ErrorMessage = "Score must be between 1 and 10")]
        public int Score { get; set; }

        public ProductToReturnDto ReviewedProduct { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow.ToLocalTime();

        public bool IsVerified { get; set; }
    }
}
