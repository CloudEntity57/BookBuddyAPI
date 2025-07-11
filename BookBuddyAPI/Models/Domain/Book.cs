using BookBuddyAPI.Models.DTO;
using System.Text.Json;

namespace BookBuddyAPI.Models.Domain
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; } 
        public List<User>? UsersWantToRead { get; set; }
        public List<UserBook>? UserBookJoin {  get; set; }


    }
}
