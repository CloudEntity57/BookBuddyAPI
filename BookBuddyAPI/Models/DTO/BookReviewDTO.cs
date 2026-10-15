using BookBuddyAPI.Models.Domain;

namespace BookBuddyAPI.Models.DTO
{
    public class BookReviewDTO
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public ReviewUserDTO? User { get; set; }
        public string? ApiId { get; set; }
        public string? Review { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}