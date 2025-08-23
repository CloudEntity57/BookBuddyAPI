using BookBuddyAPI.Data;
using BookBuddyAPI.Models.Domain;
using BookBuddyAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookBuddyAPI.Repositories
{
    public class SQLConversationRepository : IConversationRepository
    {
        private readonly BookBuddyGeneralDbContext _context;

        public SQLConversationRepository(BookBuddyGeneralDbContext context)
        {
            _context = context;
        }

        // GET: Conversation by Id
        public async Task<Conversation?> GetConversationAsync(Guid id)
        {
            return await _context.Conversations
                .AsNoTracking() // Optimization for read-only queries
                .FirstOrDefaultAsync(c => c.Id == id);
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
    }
}
