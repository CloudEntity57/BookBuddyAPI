using BookBuddyAPI.Models.Domain;
using BookBuddyAPI.Models.DTO;

namespace BookBuddyAPI.Models.DTO
{
    public class UserBookDTO
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }

        public DateTime DateAdded { get; set; }
        public string? Note { get; set; }
    }
}
