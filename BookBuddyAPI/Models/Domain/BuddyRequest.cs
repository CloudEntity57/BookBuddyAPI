namespace BookBuddyAPI.Models.Domain
{
    public class BuddyRequest
    {
        public Guid ActiveUserId { get; set; }
        public Guid PassiveUserId { get; set; }
        public string? BookTitle { get; set; }
        public string? Note {  get; set; }
        public DateTime DateAdded { get; set; }
        // navigation properties
        public User? ActiveUser { get; set; }
        public User? PassiveUser { get; set; }
    }
}
