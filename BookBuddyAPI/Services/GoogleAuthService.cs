using BookbuddyAPI.Services;
using BookBuddyAPI.Models.Domain;
using BookBuddyAPI.Repositories;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace BookBuddyAPI.Services
{
    public class GoogleAuthService: IExternalAuthService
    {
        private readonly OAuthSettings _settings;
        private readonly IDistributedCache _cache;
        private readonly IBuddyRepository _buddyRepository;
        private readonly IGoogleOAuthClient _googleClient;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IExternalLoginRepository _externalLoginRepository;
        public GoogleAuthService(IJwtService jwtService, IUserRepository userRepository, IOptions<OAuthSettings> options, IDistributedCache cache, IExternalLoginRepository externalLoginRepository, IGoogleOAuthClient googleClient)
        {
            _settings = options.Value;
            _cache = cache;
            _userRepository = userRepository;
            _jwtService = jwtService;
            _googleClient = googleClient;
            _externalLoginRepository = externalLoginRepository;
        }
        public async Task<string> GenerateAuthorizationUrlAsync()
        {
                var state = Guid.NewGuid().ToString("N");
                Console.WriteLine($"Generated state: {state}"); // Debugging line

                var pkce = PkceGenerator.Generate();
                var codeVerifier = pkce.CodeVerifier;
                Console.WriteLine($"Generated code verifier: {codeVerifier}"); // Debugging line

                var codeChallenge = pkce.CodeChallenge;
                Console.WriteLine($"Generated code challenge: {codeChallenge}"); // Debugging line
                await _cache.RemoveAsync($"oauth:{state}"); // Clear cache before setting new value

                // Store for callback validation
                await _cache.SetStringAsync(
                    $"oauth:{state}",
                    codeVerifier,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    }
                );

                var query = new Dictionary<string, string>
                {
                    ["client_id"] = _settings.ClientId,
                    ["redirect_uri"] = _settings.RedirectUri,
                    ["response_type"] = "code",
                    ["scope"] = "openid profile email",
                    ["state"] = state,
                    ["code_challenge"] = codeChallenge,
                    ["code_challenge_method"] = "S256",
                    ["access_type"] = "offline",
                    ["prompt"] = "consent"
                };

                return QueryHelpers.AddQueryString(
                    "https://accounts.google.com/o/oauth2/v2/auth",
                    query
                );
        }

        public async Task<User> GetOrCreateBookBuddyUserAsync(GoogleUser googleUser)
        {
            var externalLogin = await _externalLoginRepository.GetUserAsync(googleUser.Sub, "Google");
            if(externalLogin != null)
            {
                var existingUser = await _userRepository.GetUserByIdAsync(externalLogin.UserId);
                if(existingUser != null)
                    return existingUser;
            }
            
            var user = await _userRepository.GetUserByEmailAsync(
                    googleUser.Email);

            if (user != null)
            {
                var newlogin = new ExternalLogin
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Provider = "Google",
                    ProviderUserId = googleUser.Sub
                };
                await _externalLoginRepository.CreateAsync(newlogin);
                return user;
            }

            user = new User
            {
                Id = Guid.NewGuid(),
                Email = googleUser.Email,
                UserName = googleUser.Name,
                AvatarUrl = googleUser.Picture,
                CreatedAt = DateTime.UtcNow,
                Roles = new List<string> { "User" }
            };

            await _userRepository.CreateAsync(user);

            var login = new ExternalLogin
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Provider = "Google",
                ProviderUserId = googleUser.Sub
            };
            await _externalLoginRepository.CreateAsync(login);

            return user;
        }
        public async Task<string> LoginWithGoogleAsync(
            string code,
            string state)
        {
            // Retrieve PKCE verifier
            var codeVerifier =
                await _cache.GetStringAsync($"oauth:{state}");
            Console.WriteLine($"STATE CALLBACK: {state}");
            Console.WriteLine($"CODE VERIFIER RETRIEVED: {codeVerifier}");
            if (string.IsNullOrEmpty(codeVerifier))
                throw new Exception("No code verifier found.");

            await _cache.RemoveAsync($"oauth:{state}");

            // Exchange authorization code
            GoogleTokenResponse token = await _googleClient.ExchangeCodeForTokenAsync(
                    code,
                    codeVerifier);

            // Retrieve Google profile
            var googleUser =
                await _googleClient.GetGoogleProfileAsync(
                    token.AccessToken);

            // Find or create BookBuddy user
            var user =
                await GetOrCreateBookBuddyUserAsync(googleUser);

            // Generate BookBuddy JWT
            return _jwtService.GenerateToken(user);
        }

    }
}