using BookBuddyAPI.Models.Domain;
using Microsoft.VisualBasic;

namespace BookBuddyAPI.Models.Domain
{
    public class ConversationMember
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid ConversationId { get; set; }
        public Conversation Conversation { get; set; } = null!;

        // Role can be useful for group chats (admin, member, etc.)
        public string Role { get; set; } = "Member";

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
