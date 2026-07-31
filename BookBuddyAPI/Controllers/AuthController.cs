using BookbuddyAPI.Services;
using BookBuddyAPI.Models.DTO;
using BookBuddyAPI.Repositories;
using BookBuddyAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BookBuddyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        private readonly IExternalAuthService _googleAuthService;
        private readonly OAuthSettings _settings;

        public AuthController(ITokenRepository tokenRepository, IExternalAuthService googleAuthService, IOptions<OAuthSettings> options)
        {
            // this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this._googleAuthService = googleAuthService;
            this._settings = options.Value;
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
        [Route("google/login")]
        public async Task<IActionResult> GoogleLogin()
        {
            var redirectUrl = await _googleAuthService.GenerateAuthorizationUrlAsync();

            return Redirect(redirectUrl);
        }

        [HttpGet]
        [Route("google/callback")]
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
    }
}
