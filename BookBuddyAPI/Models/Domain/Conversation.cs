namespace BookBuddyAPI.Models.Domain
{
    public class Conversation
    {
        public Guid Id { get; set; }

        // Optional: If it's a group chat, we can store a name & image
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }

        // Helps differentiate between 1-on-1 and group chats
        public bool IsGroup { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<ConversationMember> Members { get; set; } = new List<ConversationMember>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
