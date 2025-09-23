using BookBuddyAPI.Models.DTO;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace BookBuddyAPI.Models.Domain
{
    public class Book
    {
        public Guid Id { get; set; }
        [NotMapped]
        public string ApiId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        [NotMapped]
        public List<User>? UsersWantToRead { get; set; }
        public List<UserBook>? UserBookJoin {  get; set; }


    }

}
