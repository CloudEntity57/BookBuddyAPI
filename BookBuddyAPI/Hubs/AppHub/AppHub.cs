using BookBuddyAPI.Models.Domain;
using BookBuddyAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace BookBuddyAPI.Hubs.AppHub
{
    [Authorize]
    public class AppHub : Hub
    {
        public async Task JoinUserGroup(string userId)
        {
            // Add the connection to a group based on user email
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        }
        public async Task LeaveUserGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
        }

        // Called when a user opens a conversation
        public async Task JoinConversation(string conversationId)
        {
            string groupName = $"Conversation_{conversationId}";
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            // Optional: notify others the user joined
            await Clients.Group(groupName).SendAsync("UserJoined", Context.UserIdentifier);
            Debug.WriteLine($"Added user to {groupName}");
        }

        public async Task LeaveConversation(string conversationId)
        {
            string groupName = $"Conversation_{conversationId}";
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            Debug.WriteLine($"removed user from {groupName}");
        }
        public override async Task OnConnectedAsync()
        {
            // Get the user's email from the JWT token
            var userGuid = Context.User?.Claims?.FirstOrDefault(c => c.Type == "user_guid")?.Value;
            foreach(var claim in Context.User?.Claims?.ToList())
            {
                Debug.WriteLine($"On Connected claim: {claim}");
            }
            Debug.WriteLine($"OnConnectedAsync userId: {userGuid}");

            if (!string.IsNullOrEmpty(userGuid))
            {
                Debug.WriteLine($"OnConnectedAsync userId: {userGuid}");
                // Automatically join the user to their personal group
                JoinUserGroup(userGuid);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userGuid = Context.User?.Claims?.FirstOrDefault(c => c.Type == "user_guid")?.Value;

            if (!string.IsNullOrEmpty(userGuid))
            {
                LeaveUserGroup(userGuid);
            }

            await base.OnDisconnectedAsync(exception);
        }

    }
}
