using BookBuddyAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Security.Claims;

namespace BookBuddyAPI.Hubs.Notifications
{
    [Authorize]
    public class NotificationHub : Hub
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
                //await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
                JoinUserGroup(userGuid);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userGuid = Context.User?.Claims?.FirstOrDefault(c => c.Type == "user_guid")?.Value;

            if (!string.IsNullOrEmpty(userGuid))
            {
                //await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
                LeaveUserGroup(userGuid);
            }

            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendNotification(NotificationDTO notificationDTO)
        {
            var userId = Context.User.Claims.ToList();
            //await Clients.User([recipientGuid.toString()])?.SendAsync("NewNotification", notificationDTO);
            Debug.WriteLine($"Sending a notification with GUID: {userId}");
            //await Clients.User(userId).SendAsync("NewNotification", notificationDTO);
        }
    }
}
