using AutoMapper;
using BookBuddyAPI.Models.Domain;
using BookBuddyAPI.Models.DTO;
using BookBuddyAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly INotificationsRepository repository;

        public NotificationsController(IMapper mapper, INotificationsRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        [HttpPost]
        //POST: api/notifications
        public async Task<IActionResult> AddNotification([FromBody] NotificationDTO notificationDTO)
        {
            Notification notificationDomainModel = mapper.Map<Notification>(notificationDTO);
            notificationDomainModel = await repository.AddNotificationAsync(notificationDomainModel);
            if (notificationDomainModel == null)
            {
                return NotFound();
            }
            return Ok(notificationDomainModel);
        }

        [HttpGet]
        [Route("{userId}")]
        //GET: api/notifications/{userId}
        public async Task<IActionResult> GetUserNotifications([FromRoute] string userId)
        {
            var id = Guid.Parse(userId);
            var notifications = await repository.GetUserNotificationsAsync(id);
            return Ok(mapper.Map<List<NotificationDTO>>(notifications));
        }

        [HttpPut]

        //PUT: api/notifications/
        [Route("{id}")]
        public async Task<IActionResult> UpdateNotification([FromRoute] string id, [FromBody] NotificationDTO notificationDTO)
        {
            var notificationDomainModel = mapper.Map<Notification>(notificationDTO);
            var guid = Guid.Parse(id);
            notificationDomainModel = await repository.UpdateNotificationAsync(guid, notificationDomainModel);
            if(notificationDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<NotificationDTO>(notificationDomainModel));
        }
    }
}
