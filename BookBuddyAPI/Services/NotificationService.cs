using BookBuddyAPI.Hubs.Notifications;
using BookBuddyAPI.Models.Domain;
using BookBuddyAPI.Models.DTO;
using Microsoft.AspNetCore.SignalR;

namespace BookBuddyAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext) { _hubContext = hubContext; }
        public async Task NotifyBuddyRequestAsync(Guid recipientId, NotificationDTO notificationDTO)
        {
            //await _hubContext.Clients.User(recipientId.ToString())
            await _hubContext.Clients.User(recipientId.ToString())
            .SendAsync("NewNotification", notificationDTO);

        }
        public async Task SendToUserAsync(Guid userId, string method, object data)
        {
            await _hubContext.Clients.Group($"user_{userId.ToString()}").SendAsync(method, data);
        }

        public async Task SendToAllAsync(string method, object data)
        {
            await _hubContext.Clients.All.SendAsync(method, data);
        }
    }
}
