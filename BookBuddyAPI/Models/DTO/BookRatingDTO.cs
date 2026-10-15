namespace BookBuddyAPI.Models.DTO
{
    public class BookRatingDTO
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string? ApiId { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}