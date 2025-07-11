using BookBuddyAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddyAPI.Models.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public List<Book>? WantToRead { get; set; }
        public List<UserBook>? WantReadJoin { get; set; }
    }
}
