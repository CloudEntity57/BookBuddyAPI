using BookBuddyAPI.Models.Domain;

namespace BookbuddyAPI.Services
{
public interface IAuthService
{
    Task<AuthResult> RegisterEmailAsync(RegisterRequest request);
    Task<string> GenerateTokenFromPassword(User user, string email, string password);
}

}