using BookBuddyAPI.Models.Domain;

namespace BookBuddyAPI.Repositories
{
    public interface IConversationRepository
    {
        Task<Conversation?> GetConversationAsync(Guid id);
        Task<bool> DeleteConversationAsync(Guid id);
        Task<Conversation?> CreateConversationAsync(Conversation conversation);
        Task<Conversation?> UpdateConversationAsync(Conversation conversation);
    }
}
