namespace BookBuddyAPI.Models.Domain
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid RecipientId { get; set; }
        public Guid? ActorId { get; set; }
        public NotificationType Type { get; set; }
        public string? Message { get; set; }
        public Guid? RelatedEntityId { get; set; } // e.g., BookId, PostId
        public bool IsRead { get; set; } = false;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    }

    public enum NotificationType
    {
        BuddyRequest,
        MeetingInvite,
        BuddyPosted,
        BuddyReadBook,
        SystemAnnouncement,
        MessageReceived,
        MessageSent
    }
}
