using OnlineShop.Data.Models.Identity;

namespace OnlineShop.Services.Data.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user, string role);
    }
}
