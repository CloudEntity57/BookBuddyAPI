using BookBuddyAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using BookBuddyAPI.Models.DTO;
using BookBuddyAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookBuddyAPI.Controllers
{
    // /api/users
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUserRepository repository;
        private readonly ILogger<UsersController> logger;

        public UsersController(IMapper mapper, IUserRepository repository, ILogger<UsersController> logger)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.logger = logger;
        }



        [HttpGet]
        [Route("email/{email}")]
        public async Task<IActionResult> GetUserByEmail([FromRoute] string email)
        {
            var userDomainModel = await repository.GetUserByEmailAsync(email);

            if (userDomainModel == null)
            {
                return NotFound();
            }
            //return Ok(mapper.Map<UserDTO>(userDomainModel));
            var userDto = mapper.Map<UserDTO>(userDomainModel);
            userDto.ProfileImageUrl = userDomainModel.ProfileImage != null
                ? $"{Request.Scheme}://{Request.Host}/api/users/profile-image/{userDomainModel.Id}/image"
                : null;
            return Ok(userDto);
        }

        [HttpGet]
        [Route("id/{userId}")]
        public async Task<IActionResult> GetUserById([FromRoute] string userId)
        {
            var userGuid = Guid.Parse(userId);
            var userDomainModel = await repository.GetUserByIdAsync(userGuid);

            if (userDomainModel == null)
            {
                return NotFound();
            }
            //return Ok(mapper.Map<UserDTO>(userDomainModel));
            var userDto = mapper.Map<UserDTO>(userDomainModel);
            userDto.ProfileImageUrl = userDomainModel.ProfileImage != null
                ? $"{Request.Scheme}://{Request.Host}/api/users/profile-image/{userDomainModel.Id}/image"
                : null;
            return Ok(userDto);
        }

        [HttpPost]
        // POST: api/Users
        public async Task<IActionResult> CreateNewUser([FromBody] AddUserDTO userDTO)
        {
            var userDomainModel = mapper.Map<User>(userDTO);
            userDomainModel = await repository.CreateAsync(userDomainModel);
            return Ok(mapper.Map<UserDTO>(userDomainModel));
        }

        [HttpPost]
        [Route("upload-image/{id}")]
        // POST: api/Users/upload-image/{id}
        public async Task<IActionResult> UploadProfileImage(Guid id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                await repository.SaveProfileIMage(id, file);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error saving profile image:{ex}");
                return NotFound();
            }

            return Ok(new { message = "Profile image uploaded successfully. :)" });
        }
        [HttpGet]
        [Route("profile-image/{id}")]
        public async Task<IActionResult> GetProfileImage(Guid id)
        {
            var user = await repository.GetUserByIdAsync(id);

            if (user == null || user.ProfileImage == null)
                return NotFound();
            //var profileImage = user.ProfileImage != null ? user.ProfileImage : [Convert.ToByte(255)];

            return File(user.ProfileImage, user.ProfileImageMimeType ?? "image/png");
        }
    }
}
