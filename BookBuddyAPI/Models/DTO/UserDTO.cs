using BookBuddyAPI.Models.Domain;

namespace BookBuddyAPI.Models.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string UserName { get; set; }
        public string? UserMessage { get; set; }
        public string? BookOfInterest { get; set; }
        public string? Email { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        //public List<BuddyDTO>? Buddies { get; set; }
        public List<BookDTO>? WantToRead { get; set; }
        public List<UserDTO>? ReceivedBuddyRequests { get; set; }
        public List<UserDTO>? SentBuddyRequests { get; set; }

        public List<ConversationMember>? ConversationMembers { get; set; } 
        public List<Message>? Messages { get; set; } 
        public List<MessageReaction>? MessageReactions { get; set; } 
    }
}
