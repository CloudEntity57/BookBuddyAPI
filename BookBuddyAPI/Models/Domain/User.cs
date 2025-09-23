using BookBuddyAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string? UserMessage { get; set; }
        public string? BookOfInterest { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public byte[]? ProfileImage { get; set; }  // Will map to VARBINARY(MAX)
        public string? ProfileImageMimeType { get; set; }
        public List<UserBook>? WantReadJoin { get; set; }
        [NotMapped]
        public List<Book>? WantToRead { get; set; }
        [NotMapped]
        public List<Book>? HaveRead { get; set; }
        //public List<Buddy> Buddies { get; set; }
        public List<User>? SentBuddyRequests { get; set; }
        public List<User>? ReceivedBuddyRequests { get; set; }
        public List<BuddyRequest>? SentBuddyRequestsJoin { get; set; }
        public List<BuddyRequest>? ReceivedBuddyRequestsJoin { get; set; }
        //public List<Book>? HaveRead { get; set; }
        public ICollection<ConversationMember> ConversationMembers { get; set; } = new List<ConversationMember>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<MessageReaction> MessageReactions { get; set; } = new List<MessageReaction>();

    }
}
