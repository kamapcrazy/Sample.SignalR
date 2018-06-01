using Microsoft.AspNetCore.SignalR;
using Sample.SignalR.Services.Interfaces;
using System.Threading.Tasks;

namespace Sample.SignalR.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub, INotificationHubClient> _hubContext;

        public NotificationService(IHubContext<NotificationHub, INotificationHubClient> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendPublicMessage(string message)
        {
            await ForAllClients().OnPublicMessageSent(message);
        }

        public async Task SendMessageToGroup(string message, string groupName = "")
        {
            if (string.IsNullOrEmpty(groupName))
                groupName = NotificationHub.ServerGroup;

            await ForGroup(groupName).OnPrivateMessageSent(message);
        }

        private INotificationHubClient ForGroup(string groupName)
        {
            return _hubContext.Clients.Group(groupName);
        }

        private INotificationHubClient ForAllClients()
        {
            return _hubContext.Clients.All;
        }
    }
}
