namespace BookBuddyAPI.Models.Domain
{
    public class Buddy
    {
        public Guid UserAId { get; set; }
        public User UserA { get; set; }

        public Guid UserBId { get; set; }
        public User UserB { get; set; }

        public DateTime FriendsSince {  get; set; } = DateTime.UtcNow;
    }
}
