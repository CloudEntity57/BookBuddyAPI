using BookBuddyAPI.Data;
using BookBuddyAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookBuddyAPI.Repositories
{
    public class SQLBuddyRepository : IBuddyRepository
    {
        private readonly BookBuddyGeneralDbContext dbContext;
        private readonly ILogger<SQLBuddyRepository> logger;

        public SQLBuddyRepository(BookBuddyGeneralDbContext dbContext, ILogger<SQLBuddyRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<Buddy> CreateBuddyAsync(Buddy buddyDomainModel)
        {
            var friendship = new Buddy
            {
                UserAId = (buddyDomainModel.UserAId.CompareTo(buddyDomainModel.UserBId) < 0) ? buddyDomainModel.UserAId : buddyDomainModel.UserBId,
                UserBId = (buddyDomainModel.UserAId.CompareTo(buddyDomainModel.UserBId) < 0) ? buddyDomainModel.UserBId : buddyDomainModel.UserAId,
                FriendsSince = DateTime.UtcNow
            };

            await dbContext.AddAsync(friendship);
            await dbContext.SaveChangesAsync();

            // Delete buddy request notification:
            var existingBuddyRequestNotification = await dbContext.Notifications.FirstOrDefaultAsync(n => n.ActorId == buddyDomainModel.UserAId && n.RecipientId == buddyDomainModel.UserBId && n.Type == NotificationType.BuddyRequest);
            if (existingBuddyRequestNotification != null)
            {
                dbContext.Notifications.Remove(existingBuddyRequestNotification);
                await dbContext.SaveChangesAsync();
            }
            return buddyDomainModel;
        }

        public async Task<BuddyRequest> CreateBuddyRequestAsync(BuddyRequest buddyRequestDomainModel)
        {
            await dbContext.AddAsync(buddyRequestDomainModel);
            await dbContext.SaveChangesAsync();

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
            await dbContext.Notifications.AddAsync(notification);
            await dbContext.SaveChangesAsync();
            return buddyRequestDomainModel;   
        }

        public async Task<BuddyRequest> DeleteBuddyRequestAsync(BuddyRequest buddyDomainModel)
        {
            var existingBuddyRequest = await dbContext.BuddyRequest.FirstOrDefaultAsync(x => x.ActiveUserId == buddyDomainModel.ActiveUserId && x.PassiveUserId == buddyDomainModel.PassiveUserId);
            if(existingBuddyRequest == null)
            {
                return null;
            }
            dbContext.Remove(existingBuddyRequest);
            await dbContext.SaveChangesAsync();

            // Delete buddy request notification:
            var existingBuddyRequestNotification = await dbContext.Notifications.FirstOrDefaultAsync(n => n.ActorId == buddyDomainModel.ActiveUserId && n.RecipientId == buddyDomainModel.PassiveUserId && n.Type == NotificationType.BuddyRequest);
            if(existingBuddyRequestNotification != null)
            {
                dbContext.Notifications.Remove(existingBuddyRequestNotification);
                await dbContext.SaveChangesAsync();
            }


            return existingBuddyRequest;
        }

        public async Task<List<User>> GetBuddiesAsync(Guid userId)
        {
            var buddies = await dbContext.Buddies
                .Where(b => b.UserAId == userId || b.UserBId == userId)
                .Select(b => b.UserAId == userId ? b.UserB : b.UserA)
                .ToListAsync();
            if(buddies == null)
            {
                return null;
            }
            return buddies;
        }
    }
}
