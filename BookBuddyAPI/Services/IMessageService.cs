using BookBuddyAPI.Models.DTO;

namespace BookBuddyAPI.Services
{
    public interface IMessageService
    {
        Task UpdateMessageAsync(MessageDTO message);
    }
}
