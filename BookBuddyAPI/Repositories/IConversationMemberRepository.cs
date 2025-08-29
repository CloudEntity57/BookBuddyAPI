using BookBuddyAPI.Models.Domain;

namespace BookBuddyAPI.Repositories
{
    public interface IConversationMemberRepository
    {
        Task<ConversationMember?> GetConversationMemberAsync(Guid userId);
        Task<bool> DeleteConversationMemberAsync(Guid userId);
        Task<ConversationMember?> CreateConversationMemberAsync(ConversationMember conversationMember);
        Task<ConversationMember?> UpdateConversationMemberAsync(ConversationMember conversationMember);
    }
}

