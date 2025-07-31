namespace BookBuddyAPI.Models.Domain
{
    public class UserBook
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }

        public DateTime? DateAdded { get; set; }
        public string? Note {  get; set; }

        // navigation properties
        public User? User { get; set; }

        public Book? Book { get; set; }


    }
}
