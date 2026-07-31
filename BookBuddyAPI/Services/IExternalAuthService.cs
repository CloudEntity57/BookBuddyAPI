using BookBuddyAPI.Models.Domain;

namespace BookbuddyAPI.Services
{
    public interface IExternalAuthService
    {
        Task<string> GenerateAuthorizationUrlAsync();
        Task<User> GetOrCreateBookBuddyUserAsync(GoogleUser user);
        Task<string> LoginWithGoogleAsync(string code, string state);
    }
}