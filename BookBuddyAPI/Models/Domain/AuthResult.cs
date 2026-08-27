namespace BookBuddyAPI.Models.Domain
{
    public class AuthResult
    {
        public string Token { get; set; }
        public AuthProvider Provider { get; set; }

    }

}