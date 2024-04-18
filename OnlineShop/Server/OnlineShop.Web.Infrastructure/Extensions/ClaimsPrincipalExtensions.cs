using System.Security.Claims;

namespace OnlineShop.Web.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetEmail(this ClaimsPrincipal user) 
        {
            return user.FindFirstValue(ClaimTypes.Email);
        }
        public static string GetId(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
