using Microsoft.AspNetCore.Identity;

namespace BookBuddyAPI.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
