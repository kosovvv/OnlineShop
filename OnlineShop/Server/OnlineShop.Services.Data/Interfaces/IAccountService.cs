using OnlineShop.Data.Models.Identity;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Address;
using System.Security.Claims;

namespace OnlineShop.Services.Data.Interfaces
{
    public interface IAccountService
    {
        Task<UserDto> Register(RegisterDto user);
        Task<UserDto> Login(LoginDto user);
        Task<ReturnAddressDto> UpdateUserAdresss(ClaimsPrincipal User, ReturnAddressDto address);
        Task<ReturnAddressDto> GetUserAddress(ClaimsPrincipal User);
        Task<bool> CheckEmailExistsAsync(string email);
        Task<UserDto> GetCurrentUser(ClaimsPrincipal user);
    }
}
