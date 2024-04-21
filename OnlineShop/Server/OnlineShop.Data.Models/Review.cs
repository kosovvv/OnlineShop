using System.ComponentModel.DataAnnotations;
using OnlineShop.Data.Common.Models;
using OnlineShop.Data.Models.Identity;
using OnlineShop.Models;

namespace OnlineShop.Data.Models
{
    public class Review : BaseModel<int>
    {
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        [Required(ErrorMessage = "Score is required")]
        [Range(1, 10, ErrorMessage = "Score must be between 1 and 10")]
        public int Score { get; set; }

        [Required(ErrorMessage = "Reviewed product ID is required")]
        public int ReviewedProductId { get; set; }
        public Product ReviewedProduct { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        public bool IsVerified { get; set; }
    }
}
