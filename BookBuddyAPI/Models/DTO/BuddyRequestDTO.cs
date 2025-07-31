using BookBuddyAPI.Models.Domain;

namespace BookBuddyAPI.Models.DTO
{
    public class BuddyRequestDTO
    {
        public Guid ActiveUserId { get; set; }
        public Guid PassiveUserId { get; set; }
        public string? BookTitle { get; set; }
        public string? Note { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        // navigation properties
        //public UserDTO? ActiveUser { get; set; }
        //public UserDTO? PassiveUser { get; set; }
    }
}
