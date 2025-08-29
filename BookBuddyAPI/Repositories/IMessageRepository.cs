using BookBuddyAPI.Models.Domain;

namespace BookBuddyAPI.Repositories
{
    public interface IMessageRepository
    {
        Task<Message?> CreateMessageAsync(Message message);
        Task<Message?> GetMessageAsync(Guid id);
        Task<Message?> UpdateMessageAsync(Message message);
        Task<bool> DeleteMessageAsync(Guid id);
    }
}
