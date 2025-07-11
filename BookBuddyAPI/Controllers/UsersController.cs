using BookBuddyAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using BookBuddyAPI.Models.DTO;
using BookBuddyAPI.Models.Domain;

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
        [Route("{email}")]
        public async Task<IActionResult> GetUserByEmail([FromRoute] string email)
        {
            var userDomainModel = await repository.GetUserByEmailAsync(email);

            if (userDomainModel == null)
            {
                return NotFound();
            }
            //return Ok(mapper.Map<UserDTO>(userDomainModel));
            return Ok(mapper.Map<UserDTO>(userDomainModel));
        }

        [HttpPost]
        // POST: api/Users
        public async Task<IActionResult> CreateNewUser([FromBody] AddUserDTO userDTO)
        {
            var userDomainModel = mapper.Map<User>(userDTO);
            userDomainModel = await repository.CreateAsync(userDomainModel);
            return Ok(mapper.Map<UserDTO>(userDomainModel));
        }
    }
}
