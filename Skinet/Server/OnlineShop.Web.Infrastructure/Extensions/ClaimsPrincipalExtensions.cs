using System.Security.Claims;

namespace OnlineShop.Web.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static string RetrieveEmailFromPrincipal(this ClaimsPrincipal user) 
        {
            return user.FindFirstValue(ClaimTypes.Email);
        }
    }
}
