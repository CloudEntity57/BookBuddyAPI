using BookBuddyAPI.Data;
using BookBuddyAPI.Models.Domain;
using BookBuddyAPI.Models.DTO;
using BookBuddyAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookBuddyAPI.Repositories
{
    public class SQLConversationRepository : IConversationRepository
    {
        private readonly BookBuddyGeneralDbContext _context;
        private readonly ILogger<SQLBookRepository> logger;

        public SQLConversationRepository(BookBuddyGeneralDbContext context, ILogger<SQLBookRepository> logger)
        {
            _context = context;
            this.logger = logger;
        }

        // GET: Conversation by Id
        public async Task<Conversation?> GetConversationAsync(Guid id)
        {
            var conversation = await _context.Conversations
                .Include(c => c.Messages)
                .AsNoTracking() // Optimization for read-only queries
                .FirstOrDefaultAsync(c => c.Id == id);

            logger.LogInformation($"Loaded messages: {conversation.Messages.Count}");
            return conversation;
        }

        // CREATE: Add new Conversation
        public async Task<Conversation?> CreateConversationAsync(Conversation conversation)
        {
            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();
            return conversation;
        }

        // UPDATE: Modify an existing Conversation
        public async Task<Conversation?> UpdateConversationAsync(Conversation conversation)
        {
            var existingConversation = await _context.Conversations.FindAsync(conversation.Id);
            if (existingConversation == null)
                return null;

            // Map updated fields (manual mapping; AutoMapper handles this in controllers)
            _context.Entry(existingConversation).CurrentValues.SetValues(conversation);

            await _context.SaveChangesAsync();
            return existingConversation;
        }

        // DELETE: Remove Conversation by Id
        public async Task<bool> DeleteConversationAsync(Guid id)
        {
            var conversation = await _context.Conversations.FindAsync(id);
            if (conversation == null)
                return false;

            _context.Conversations.Remove(conversation);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Conversation?> GetPrivateConversationBetweenUsersAsync(Guid id1, Guid id2)
        {
            return await _context.Conversations
            .Include(c => c.Members)
            .Where(c => c.Members.Any(p => p.UserId == id1) &&
                        c.Members.Any(p => p.UserId == id2) &&
                        c.IsGroup == false)
            .Include(c => c.Messages)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        }
    }
}
