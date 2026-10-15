namespace BookBuddyAPI.Models.DTO
{
    public class UserBookReviewDTO
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string? ApiId { get; set; }
        public string? Review { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}