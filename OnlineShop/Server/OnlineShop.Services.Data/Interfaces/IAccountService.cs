using OnlineShop.Data.Models.Identity;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Address;
using System.Security.Claims;

namespace OnlineShop.Services.Data.Interfaces
{
    public interface IAccountService
    {
        Task<ReturnUserDto> Register(RegisterDto user);
        Task<ReturnUserDto> Login(LoginDto user);
        Task<ReturnAddressDto> UpdateUserAdresss(string userId, ReturnAddressDto address);
        Task<ReturnAddressDto> GetUserAddress(string userId);
        Task<bool> CheckEmailExistsAsync(string email);
        Task<ReturnUserDto> GetCurrentUser(string userId);
    }
}
