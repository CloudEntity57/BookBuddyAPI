using BookBuddyAPI.Models.Domain;

namespace BookBuddyAPI.Models.DTO
{
    public class BookDTO
    {
        public Guid Id { get; set; }
        public string? ApiId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        //public UserBookDTO? UserBookJoin { get; set; }
        public List<UserDTO>? UsersWantToRead { get; set; }
    }
}
