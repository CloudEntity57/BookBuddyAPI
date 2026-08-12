using System.Net.Http.Headers;
using System.Text.Json;
using BookbuddyAPI.Services;
using Microsoft.Extensions.Options;

namespace BookBuddyAPI.Services
{

    public class GoogleOAuthClient : IGoogleOAuthClient
    {
        private readonly HttpClient _httpClient;
        private readonly OAuthSettings _settings;

        public GoogleOAuthClient(
            HttpClient httpClient,
            IOptions<OAuthSettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }

        public async Task<GoogleTokenResponse> ExchangeCodeForTokenAsync(
            string code,
            string codeVerifier
        )
        {
            var request = new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["client_id"] = _settings.ClientId,
                ["client_secret"] = _settings.ClientSecret,
                ["redirect_uri"] = _settings.RedirectUri,
                ["code_verifier"] = codeVerifier
            };

            Console.WriteLine($"Exchanging code for token with request: {JsonSerializer.Serialize(request)}"); // Debugging line

            var response = await _httpClient.PostAsync(
                "https://oauth2.googleapis.com/token",
                new FormUrlEncodedContent(request));

            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(body);

            return JsonSerializer.Deserialize<GoogleTokenResponse>(
                body,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        }
        public async Task<GoogleUser> GetGoogleProfileAsync(
            string accessToken)
        {
            Console.WriteLine($"Fetching Google profile with access token: {accessToken}"); // Debugging line
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    accessToken);

            var response = await _httpClient.GetAsync(
                "https://openidconnect.googleapis.com/v1/userinfo");

            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(body);

            return JsonSerializer.Deserialize<GoogleUser>(
                body,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        }    }

}