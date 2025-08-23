using BookBuddyAPI.Models.Domain;

namespace BookBuddyAPI.Models.DTO
{
    public class ConversationDTO
    {
        public Guid Id { get; set; }

        // Optional: If it's a group chat, we can store a name & image
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }

        // Helps differentiate between 1-on-1 and group chats
        public bool IsGroup { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<MessageDTO> Messages { get; set; } = new List<MessageDTO>();
        public ICollection<ConversationMember> Members { get; set; } = new List<ConversationMember>();
    }
}
