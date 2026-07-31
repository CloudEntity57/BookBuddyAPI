using BookBuddyAPI.Models.Domain;

namespace BookBuddyAPI.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
