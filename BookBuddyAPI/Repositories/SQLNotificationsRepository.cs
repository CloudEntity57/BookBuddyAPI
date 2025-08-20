using BookBuddyAPI.Data;
using BookBuddyAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookBuddyAPI.Repositories
{
    public class SQLNotificationsRepository : INotificationsRepository
    {
        private readonly BookBuddyGeneralDbContext dbContext;

        public SQLNotificationsRepository(BookBuddyGeneralDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Notification> AddNotificationAsync(Notification notificationDomainModel)
        {
            switch (notificationDomainModel.Type)
            {
                case NotificationType.BuddyRequest:
                    await dbContext.AddAsync(notificationDomainModel);
                    await dbContext.SaveChangesAsync();
                    return notificationDomainModel;
                default:
                    await dbContext.AddAsync(notificationDomainModel);
                    await dbContext.SaveChangesAsync();
                    return notificationDomainModel;
            }
        }

        public Task<Notification> DeleteNotificationAsync(Guid notificationId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(Guid userId)
        {
            var notifications = await dbContext.Notifications.Where(n => n.RecipientId == userId).ToListAsync();
            if (notifications == null)
            {
                return [];
            }
            return notifications;
        }

        public async Task<Notification> UpdateNotificationAsync(Guid notificationId, Notification notificationDomainModel)
        {
            var existingNotification = await dbContext.Notifications.FirstOrDefaultAsync(n => n.Id == notificationId);
            if (existingNotification == null)
            {
                return null;
            }
            existingNotification.IsRead = notificationDomainModel.IsRead;

            await dbContext.SaveChangesAsync();
            return existingNotification;
        }
    }
}
