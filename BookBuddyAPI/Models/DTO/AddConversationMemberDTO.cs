namespace BookBuddyAPI.Models.DTO
{
    public class AddConversationMemberDTO
    {
        public Guid UserId { get; set; }
        public string? UserName { get; set; }

        public Guid ConversationId { get; set; }

        // Role can be useful for group chats (admin, member, etc.)
        public string Role { get; set; } = "Member";

        //public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
