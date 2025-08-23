using BookBuddyAPI.Data;
using BookBuddyAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace BookBuddyAPI.Repositories
{
    public class SQLMessageRepository : IMessageRepository
    {
        private readonly BookBuddyGeneralDbContext _context;

        public SQLMessageRepository(BookBuddyGeneralDbContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task<Message?> CreateMessageAsync(Message message)
        {
            try
            {
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                // Fetch the latest persisted message to return
                return await _context.Messages
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == message.Id);
            }
            catch
            {
                return null; // In production, log the exception
            }
        }

        // READ (by ID)
        public async Task<Message?> GetMessageAsync(Guid id)
        {
            return await _context.Messages
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        // UPDATE
        public async Task<Message?> UpdateMessageAsync(Message message)
        {
            var existingMessage = await _context.Messages.FirstOrDefaultAsync(m => m.Id == message.Id);

            if (existingMessage == null)
                return null;

            // Update fields
            existingMessage.Content = message.Content;
            existingMessage.SentAt = message.SentAt;
            existingMessage.SenderId = message.SenderId;
            existingMessage.ConversationId = message.ConversationId;

            try
            {
                await _context.SaveChangesAsync();

                // Return updated entity from database
                return await _context.Messages
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == message.Id);
            }
            catch
            {
                return null; // In production, log the exception
            }
        }

        // DELETE
        public async Task<bool> DeleteMessageAsync(Guid id)
        {
            var existingMessage = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);

            if (existingMessage == null)
                return false;

            try
            {
                _context.Messages.Remove(existingMessage);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false; // In production, log the exception
            }
        }
    }
}
