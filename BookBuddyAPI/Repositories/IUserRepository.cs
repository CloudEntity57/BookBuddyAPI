using BookBuddyAPI.Models.Domain;

namespace BookBuddyAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User> CreateAsync(User user);
    }
}
