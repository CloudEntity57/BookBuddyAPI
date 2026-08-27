namespace BookBuddyAPI.Models.Domain
{
public class UserAuthIdentity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public AuthProvider Provider { get; set; }

    public string ProviderKey { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
}
}
