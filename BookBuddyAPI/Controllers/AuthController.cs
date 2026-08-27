using System.Security.Authentication;
using BookbuddyAPI.Services;
using BookBuddyAPI.Models.Domain;
using BookBuddyAPI.Models.DTO;
using BookBuddyAPI.Repositories;
using BookBuddyAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using NuGet.Common;

namespace BookBuddyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        private readonly IExternalAuthService _googleAuthService;
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly OAuthSettings _settings;

        public AuthController(ITokenRepository tokenRepository, IExternalAuthService googleAuthService, IAuthService authService, IOptions<OAuthSettings> options, IUserRepository userRepository)
        {
            // this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this._googleAuthService = googleAuthService;
            this._settings = options.Value;
            this._authService = authService;
            this._userRepository = userRepository;
        }

        // POST: /api/Auth/Register
        // [HttpPost]
        // [Route("Register")]
        // public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        // {
        //     var identityUser = new IdentityUser
        //     {
        //         UserName = registerRequestDto.Username,
        //         Email = registerRequestDto.Username
        //     };
        //     var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

        //     if (identityResult.Succeeded)
        //     {
        //         // Add roles to this user
        //         if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
        //         {
        //             identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
        //             if (identityResult.Succeeded)
        //             {
        //                 return Ok("User was registered! Please login.");
        //             }
        //         }
        //     }

        //     return BadRequest("Something went awry");

        // }

        // POST: /api/Auth/login

        [HttpGet]
        // GET: /api/auth/google/login
        [Route("google/login")]
        public async Task<IActionResult> GoogleLogin()
        {
            var redirectUrl = await _googleAuthService.GenerateAuthorizationUrlAsync();

            return Redirect(redirectUrl);
        }

        [HttpGet]
        [Route("google/callback")]
        // GET: /api/auth/google/callback
        public async Task<IActionResult> GoogleCallback(
            [FromQuery] string code,
            [FromQuery] string state)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return BadRequest("Authorization code was not supplied.");
            }

            string? jwt = await _googleAuthService.LoginWithGoogleAsync(code, state);
            if (string.IsNullOrEmpty(jwt))
            {
                return BadRequest("Failed to generate JWT.");
            }

            string? frontendUrl = _settings.RootUrl;

            return Redirect($"{frontendUrl}?token={Uri.EscapeDataString(jwt)}");
        }

        // Register a new User with email/username/password:

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        // POST: /api/auth/register
        public async Task<IActionResult> RegisterWithEmail(
            [FromBody] RegisterRequest request
        )
        {
            try
            {
                var result = await _authService.RegisterEmailAsync(request);
                return Ok(result);
            }
            catch (AuthenticationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        // POST /api/auth/login
        public async Task<IActionResult> LoginWithEmail(
            [FromBody] LoginRequestDto loginRequest
        )
        {   
            var email = loginRequest.Username;
            var password = loginRequest.Password;
            var user = await _userRepository.GetUserByEmailAsync(email);
            if(user == null)
            {
                return Unauthorized();
            }
            var token = await _authService.GenerateTokenFromPassword(user, email, password);
            if(token == null)
            {
                return Unauthorized();
            }
            var authResult = new AuthResult {
                Token = token,
                Provider = AuthProvider.Local
            };
            return Ok(authResult);
        }

    }
}
