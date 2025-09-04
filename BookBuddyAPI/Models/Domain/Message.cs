namespace BookBuddyAPI.Models.Domain
{
    public class Message
    {
        public Guid Id { get; set; }

        public Guid ConversationId { get; set; }
        public Conversation Conversation { get; set; } = null!;

        public Guid SenderId { get; set; }
        public User Sender { get; set; } = null!;

        public string Content { get; set; } = null!;  // Can support text, emoji, etc.
        public string? AttachmentUrl { get; set; }    // Optional for images, docs, etc.

        public DateTime SentAt { get; set; }
        public bool IsEdited { get; set; } = false;

        public ICollection<MessageReaction> Reactions { get; set; } = new List<MessageReaction>();
    }
}
