using BookBuddyAPI.Models.Domain;
using BookBuddyAPI.Models.DTO;

namespace BookBuddyAPI.Services
{
    public interface INotificationService
    {
        Task NotifyBuddyRequestAsync(Guid RecipientId, NotificationDTO notificationDTO);
        Task SendToUserAsync(Guid userId, string method, object data);
        Task SendToAllAsync(string method, object data);

    }
}
