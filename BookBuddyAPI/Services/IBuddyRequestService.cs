using BookBuddyAPI.Models.Domain;
using BookBuddyAPI.Models.DTO;

namespace BookBuddyAPI.Services
{
    public interface IBuddyRequestService {
        public Task<BuddyRequest> CreateBuddyRequestAsync(BuddyRequest buddyDomainModel);
    }
}
