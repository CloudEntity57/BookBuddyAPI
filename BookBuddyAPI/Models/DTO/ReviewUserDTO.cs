namespace BookBuddyAPI.Models.DTO;
public class ReviewUserDTO
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public byte[]? ProfileImage { get; set; }
}