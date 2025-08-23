using BookBuddyAPI.Models.Domain;

namespace BookBuddyAPI.Models.DTO
{
    public class MessageReactionDTO
    {
        public Guid Id { get; set; }

        public Guid MessageId { get; set; }
        public Message Message { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Could be a Unicode emoji like "❤️" or something like ":thumbsup:"
        public string Emoji { get; set; } = null!;

        public DateTime ReactedAt { get; set; } = DateTime.UtcNow;
    }
}
