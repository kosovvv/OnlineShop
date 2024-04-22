using System.ComponentModel.DataAnnotations;
using static OnlineShop.Common.Entities.AddressConstants;
namespace OnlineShop.Web.ViewModels
{
    public class CreateAddressDto
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
    }
}
