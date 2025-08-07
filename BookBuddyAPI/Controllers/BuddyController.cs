using AutoMapper;
using BookBuddyAPI.Models.Domain;
using BookBuddyAPI.Models.DTO;
using BookBuddyAPI.Repositories;
using BookBuddyAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BookBuddyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BuddyController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ILogger<BookController> logger;
        private readonly IBuddyRepository repository;
        private readonly INotificationsRepository notificationsRepository;
        private readonly INotificationService notificationService;
        private readonly IBuddyRequestService buddyRequestService;

        public BuddyController(IMapper mapper, ILogger<BookController> logger, IBuddyRepository repository, INotificationsRepository notificationsRepository, INotificationService notificationService, IBuddyRequestService buddyRequestService)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.repository = repository;
            this.notificationsRepository = notificationsRepository;
            this.notificationService = notificationService;
            this.buddyRequestService = buddyRequestService;
        }
        [HttpPost]
        // POST: api/buddy
        public async Task<IActionResult> AddBuddy([FromBody] BuddyDTO createBuddyDTO)
        {
            var createBuddyDomainModel = mapper.Map<Buddy>(createBuddyDTO);
            createBuddyDomainModel = await repository.CreateBuddyAsync(createBuddyDomainModel);
            if (createBuddyDomainModel == null) {
                return NotFound();
            }
            return Ok(mapper.Map<BuddyDTO>(createBuddyDomainModel));
           
        }

        [HttpDelete]
        [Route("request")]
        // DELETE: api/buddy/request
        public async Task<IActionResult> DeleteBuddyRequest([FromBody] BuddyRequestDTO buddyRequestDTO)
        {
            var buddyRequestDomainModel = mapper.Map<BuddyRequest>(buddyRequestDTO);
            buddyRequestDomainModel = await repository.DeleteBuddyRequestAsync(buddyRequestDomainModel);
            if(buddyRequestDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<BuddyRequestDTO>(buddyRequestDomainModel));
        }

        [HttpPost]
        [Route("request")]
        // POST: api/buddy/request
        public async Task<IActionResult> AddBuddyRequest([FromBody] BuddyRequestDTO buddyRequestDTO)
        {
            var buddyRequestDomainModel = mapper.Map<BuddyRequest>(buddyRequestDTO);
            //buddyRequestDomainModel = await repository.CreateBuddyRequestAsync(buddyRequestDomainModel);
            //// Add new buddy request notification:
            //var notification = new Notification
            //{
            //    Id = new Guid(),
            //    RecipientId = buddyRequestDomainModel.PassiveUserId,
            //    ActorId = buddyRequestDomainModel.ActiveUserId,
            //    Type = NotificationType.BuddyRequest,
            //    Timestamp = DateTime.UtcNow,
            //    Message = buddyRequestDomainModel.Note
            //};
            ////await notificationService.NotifyBuddyRequestAsync(buddyRequestDomainModel.PassiveUserId,mapper.Map<NotificationDTO>(notification));
            //Debug.WriteLine($"SendToUserAsync called for {buddyRequestDomainModel.PassiveUserId}");
            //await notificationService.SendToUserAsync(buddyRequestDomainModel.PassiveUserId, "NewNotification", notification);
            buddyRequestDomainModel = await buddyRequestService.CreateBuddyRequestAsync(buddyRequestDomainModel);
            if(buddyRequestDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<BuddyRequestDTO>(buddyRequestDomainModel));
        }

        [HttpGet]
        [Route("{userId}")]
        // GET: api/buddy/{userId}
        public async Task<IActionResult> GetBuddies([FromRoute] string userId)
        {
            var id = Guid.Parse(userId);
            var buddiesDomainModel = await repository.GetBuddiesAsync(id);
            if (buddiesDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<List<UserDTO>>(buddiesDomainModel));
        }
    }
}
