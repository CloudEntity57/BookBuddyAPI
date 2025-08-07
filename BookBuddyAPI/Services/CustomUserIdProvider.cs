using BookBuddyAPI.Models.Domain;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Security.Claims;

namespace BookBuddyAPI.Services
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            foreach (var claim in connection.User.Claims)
            {
                Debug.WriteLine($"Claim: {claim.Type} = {claim.Value}");
            }
            // 👇 Use a claim from your JWT token, e.g., sub (subject), or a custom "userId"
            //return connection.User?.FindFirst("sub")?.Value;
            var myUser = connection.User?.FindFirst("user_guid");
            Debug.WriteLine($"a possible user: {myUser} ");
            
            return connection.User?.FindFirst("user_guid")?.Value;
        }
    }
}
