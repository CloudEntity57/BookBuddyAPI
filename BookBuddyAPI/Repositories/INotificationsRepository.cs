using BookBuddyAPI.Models.Domain;

namespace BookBuddyAPI.Repositories
{
    public interface INotificationsRepository
    {
        Task<List<Notification>> GetUserNotificationsAsync(Guid userId);
        Task<Notification> AddNotificationAsync(Notification notificationDomainModel);
        Task<Notification> DeleteNotificationAsync(Guid notificationId);
        Task<Notification> UpdateNotificationAsync(Guid notificationId, Notification notificationDomainModel);

    }
}
