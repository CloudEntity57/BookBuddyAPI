using BookBuddyAPI.Models.Domain;

namespace BookBuddyAPI.Models.DTO
{
    public class AddMessageDTO
    {

        public Guid ConversationId { get; set; }

        public Guid SenderId { get; set; }

        public string Content { get; set; } = null!;  // Can support text, emoji, etc.
        public string? AttachmentUrl { get; set; }    // Optional for images, docs, etc.

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsEdited { get; set; } = false;

    }
}
