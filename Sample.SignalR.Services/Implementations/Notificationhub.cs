using Microsoft.AspNetCore.SignalR;
using Sample.SignalR.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Sample.SignalR.Services.Implementations
{
    public class NotificationHub : Hub<INotificationHubClient>, INotificationHubServer
    {
        public const string ServerGroup = "Server";

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, ServerGroup);

            await Clients.Caller.OnConnected($"Connected to hub and joined to default group '{ServerGroup}'.");

            await base.OnConnectedAsync();
        }

        public async Task JoinGroupAsync(string groupName)
        {
            var groupToJoin = string.IsNullOrEmpty(groupName) ? ServerGroup : groupName;

            await Groups.AddToGroupAsync(Context.ConnectionId, groupToJoin);

            await Clients.Caller.OnPrivateMessageSent($"Joined group: {groupToJoin}");
        }

        public async Task LeaveGroupAsync(string groupName)
        {
            if (!string.IsNullOrEmpty(groupName))
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Caller.OnPrivateMessageSent($"Leaved from group: {groupName}");
        }


        public async Task SendPublicMessage(string message)
        {
            await Clients.All.OnPublicMessageSent(message);
        }

        public async Task SendMessageToGroup(string groupName, string message)
        {
            if (string.IsNullOrEmpty(groupName))
                throw new ArgumentNullException(groupName);

            await Clients.Group(groupName).OnPrivateMessageSent(message);
        }
    }
}
