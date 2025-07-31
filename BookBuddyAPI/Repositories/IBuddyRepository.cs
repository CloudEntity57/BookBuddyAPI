using BookBuddyAPI.Models.Domain;

namespace BookBuddyAPI.Repositories
{
    public interface IBuddyRepository
    {
        Task<BuddyRequest> CreateBuddyRequestAsync (BuddyRequest buddyDomainModel);
        Task<BuddyRequest> DeleteBuddyRequestAsync(BuddyRequest buddyDomainModel);
        Task<Buddy> CreateBuddyAsync(Buddy buddyDomainModel);
        Task<List<User>> GetBuddiesAsync(Guid userId);
    }
}
