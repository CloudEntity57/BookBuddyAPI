using BookBuddyAPI.Models.Domain;
using BookBuddyAPI.Repositories;
using System.Diagnostics;

namespace BookBuddyAPI.Services
{
    public class BuddyRequestService : IBuddyRequestService
    {
        private readonly IBuddyRepository buddyRepository;
        private readonly INotificationsRepository notificationsRepository;
        private readonly INotificationService notificationService;

        public BuddyRequestService(IBuddyRepository buddyRepository, INotificationsRepository notificationsRepository, INotificationService notificationService)
        {
            this.buddyRepository = buddyRepository;
            this.notificationsRepository = notificationsRepository;
            this.notificationService = notificationService;
        }
        public async Task<BuddyRequest> CreateBuddyRequestAsync(BuddyRequest buddyRequestDomainModel)
        {
            // 1. Create friend request 
            if(buddyRequestDomainModel == null)
            {
                return null;
            }

            var buddyRequest = await buddyRepository.CreateBuddyRequestAsync(buddyRequestDomainModel);

            // Add new buddy request notification:
            var notification = new Notification
            {
                Id = new Guid(),
                RecipientId = buddyRequestDomainModel.PassiveUserId,
                ActorId = buddyRequestDomainModel.ActiveUserId,
                Type = NotificationType.BuddyRequest,
                Timestamp = DateTime.UtcNow,
                Message = buddyRequestDomainModel.Note
            };

            notification = await notificationsRepository.AddNotificationAsync(notification);



            // 3. Send SignalR notification 
            Debug.WriteLine($"SendToUserAsync called for {buddyRequestDomainModel.PassiveUserId}");
            await notificationService.SendToUserAsync(buddyRequestDomainModel.PassiveUserId, "NewNotification", notification);

            return buddyRequest;
        }
    }
}
