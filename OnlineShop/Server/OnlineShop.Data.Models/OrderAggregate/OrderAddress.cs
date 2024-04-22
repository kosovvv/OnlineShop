using System.ComponentModel.DataAnnotations;
using static OnlineShop.Common.Entities.OrderAddressConstants;

namespace OnlineShop.Data.Models.OrderAggregate
{
    public class OrderAddress
    {
        public OrderAddress()
        {

        }

        public OrderAddress(string firstName, string lastName, string street, string city, string state, string zipCode)
        {
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

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
