using Skinet.Core.Entities.Identity;

namespace Skinet.Core.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user);
    }
}
