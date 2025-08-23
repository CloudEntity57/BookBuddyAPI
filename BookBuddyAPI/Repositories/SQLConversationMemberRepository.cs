using BookBuddyAPI.Data;
using BookBuddyAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookBuddyAPI.Repositories
{
    public class SQLConversationMemberRepository : IConversationMemberRepository
    {
        private readonly BookBuddyGeneralDbContext _context;

        public SQLConversationMemberRepository(BookBuddyGeneralDbContext context)
        {
            _context = context;
        }

        // GET: Conversation by Id
        public async Task<ConversationMember?> GetConversationMemberAsync(Guid userId)
        {
            return await _context.ConversationMembers
                .AsNoTracking() // Optimization for read-only queries
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        // CREATE: Add new Conversation
        public async Task<ConversationMember?> CreateConversationMemberAsync(ConversationMember conversationMember)
        {
            _context.ConversationMembers.Add(conversationMember);
            await _context.SaveChangesAsync();
            return conversationMember;
        }

        // UPDATE: Modify an existing ConversationMember
        public async Task<ConversationMember?> UpdateConversationMemberAsync(ConversationMember conversationMember)
        {
            var existingConversationMember = await _context.ConversationMembers.FindAsync(conversationMember.UserId);
            if (existingConversationMember == null)
                return null;

            // Map updated fields (manual mapping; AutoMapper handles this in controllers)
            _context.Entry(existingConversationMember).CurrentValues.SetValues(conversationMember);

            await _context.SaveChangesAsync();
            return existingConversationMember;
        }

        // DELETE: Remove ConversationMember by Id
        public async Task<bool> DeleteConversationMemberAsync(Guid id)
        {
            var conversationMember = await _context.ConversationMembers.FindAsync(id);
            if (conversationMember == null)
                return false;

            _context.ConversationMembers.Remove(conversationMember);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
