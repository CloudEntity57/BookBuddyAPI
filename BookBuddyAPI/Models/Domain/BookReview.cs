namespace BookBuddyAPI.Models.Domain
{
    public class BookReview
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public string? Review { get; set; }
        public User? User { get; set; }
        public Book? Book { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}