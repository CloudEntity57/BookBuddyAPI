using BookBuddyAPI.Models.Domain;

namespace BookBuddyAPI.Models.DTO
{
    public class ConversationMemberDTO
    {
        public Guid UserId { get; set; }
        public UserDTO User { get; set; } = null!;

        public Guid ConversationId { get; set; }
        public ConversationDTO Conversation { get; set; } = null!;

        // Role can be useful for group chats (admin, member, etc.)
        public string Role { get; set; } = "Member";

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
