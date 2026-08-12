using BookBuddyAPI.Models.Domain;

namespace BookBuddyAPI.Repositories
{
    public interface IExternalLoginRepository
    {
        Task<ExternalLogin?> GetUserAsync(string providerUserId, string provider);
        Task<ExternalLogin?> CreateAsync(ExternalLogin externalLogin);
    }
}