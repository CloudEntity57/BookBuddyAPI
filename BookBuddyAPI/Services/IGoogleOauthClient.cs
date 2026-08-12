using System.Text.Json.Serialization;
using BookBuddyAPI.Services;

namespace BookbuddyAPI.Services
{
    public class GoogleUser
    {
        [JsonPropertyName("sub")]
        public string Sub { get; set; } = "";

        [JsonPropertyName("email")]
        public string Email { get; set; } = "";

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("picture")]
        public string Picture { get; set; } = "";

        [JsonPropertyName("given_name")]
        public string GivenName { get; set; } = "";

        [JsonPropertyName("family_name")]
        public string FamilyName { get; set; } = "";

        [JsonPropertyName("email_verified")]
        public bool EmailVerified { get; set; }
    }
    public class GoogleTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = "";

        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }

        [JsonPropertyName("id_token")]
        public string IdToken { get; set; } = "";

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = "";
    }
    public interface IGoogleOAuthClient
    {   
        Task<GoogleTokenResponse> ExchangeCodeForTokenAsync(
            string code,
            string codeVerifier);

        Task<GoogleUser> GetGoogleProfileAsync(
            string accessToken);
    }
}