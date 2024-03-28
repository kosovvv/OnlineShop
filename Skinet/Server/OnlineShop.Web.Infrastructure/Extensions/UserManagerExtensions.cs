using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Skinet.Core.Entities.Identity;
using System.Security.Claims;

namespace OnlineShop.Web.Infrastructure
{
    public static class UserManagerExtensions
    {
        public static async Task<ApplicationUser> FindUserByClaimsPrincipleWithAddress
            (this UserManager<ApplicationUser> userManager, ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);

            return await userManager.Users.Include(x => x.Address)
                .SingleOrDefaultAsync(x =>  x.Email == email);
        }

        public static async Task<ApplicationUser> FindByEmailFromClaimsPrincipal
            (this UserManager<ApplicationUser> userManager, ClaimsPrincipal user)
        {
            return await userManager.Users
                .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));
        }
    }
}
