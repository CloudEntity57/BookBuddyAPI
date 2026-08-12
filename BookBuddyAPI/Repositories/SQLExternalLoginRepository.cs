using BookBuddyAPI.Data;
using BookBuddyAPI.Models.Domain;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BookBuddyAPI.Repositories
{
    public class SQLExternalLoginRepository : IExternalLoginRepository
    {
        private readonly BookBuddyGeneralDbContext dbContext;
        private readonly ILogger<SQLExternalLoginRepository> logger;

        public SQLExternalLoginRepository(BookBuddyGeneralDbContext dbContext, ILogger<SQLExternalLoginRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<ExternalLogin?> GetUserAsync(string providerUserId, string provider)
        {
            return await dbContext.ExternalLogins.FirstOrDefaultAsync(el => el.ProviderUserId == providerUserId && el.Provider == provider);
        }

        public async Task<ExternalLogin?> CreateAsync(ExternalLogin externalLogin)
        {
            await dbContext.ExternalLogins.AddAsync(externalLogin);
            await dbContext.SaveChangesAsync();
            return externalLogin;
        }
    }
}