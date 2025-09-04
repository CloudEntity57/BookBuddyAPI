using BookBuddyAPI.Hubs.AppHub;
using BookBuddyAPI.Models.DTO;
using Microsoft.AspNetCore.SignalR;

namespace BookBuddyAPI.Services
{
    public class MessageService : IMessageService
    {
        private readonly IHubContext<AppHub> hubContext;

        public MessageService(IHubContext<AppHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public async Task UpdateMessageAsync(MessageDTO messageDTO)
        {
            var conversationId = messageDTO.ConversationId;
            await hubContext.Clients.Group($"Conversation_{conversationId}").SendAsync("ConversationUpdated", messageDTO);
        }
    }
}
