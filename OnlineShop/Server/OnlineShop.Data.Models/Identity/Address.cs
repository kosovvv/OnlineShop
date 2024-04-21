using System.ComponentModel.DataAnnotations;
using OnlineShop.Data.Common.Models;
using OnlineShop.Models;

namespace OnlineShop.Data.Models.Identity
{
    public class Address : BaseDeletableModel<int>
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Street address is required")]
        public string Street { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "Zip code is required")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
