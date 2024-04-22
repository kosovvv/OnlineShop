using System.ComponentModel.DataAnnotations;
using static OnlineShop.Common.Entities.AddressConstants;
using OnlineShop.Data.Common.Models;

namespace OnlineShop.Data.Models.Identity
{
    public class Address : BaseDeletableModel<int>
    {
        [Required(ErrorMessage = FirstNameRequiredMessage)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = LastNameRequiredMessage)]
        public string LastName { get; set; }

        [Required(ErrorMessage = StreetRequiredMessage)]
        public string Street { get; set; }

        [Required(ErrorMessage = CityRequiredMessage)]
        public string City { get; set; }

        [Required(ErrorMessage = StateRequiredMessage)]
        public string State { get; set; }

        [Required(ErrorMessage = ZipCodeRequiredMessage)]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = ApplicationUserIdRequiredMessage)]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
