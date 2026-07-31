namespace BookBuddyAPI.Models.Domain
{
    public class ExternalLogin
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Provider { get; set; }
        public string ProviderUserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}