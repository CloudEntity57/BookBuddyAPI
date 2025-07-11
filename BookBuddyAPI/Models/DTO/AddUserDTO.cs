namespace BookBuddyAPI.Models.DTO
{
    public class AddUserDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
}
