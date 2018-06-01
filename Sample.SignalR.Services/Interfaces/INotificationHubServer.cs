using System.Threading.Tasks;

namespace Sample.SignalR.Services.Interfaces
{
    public interface INotificationHubServer
    {
        Task JoinGroupAsync(string groupName);
        Task LeaveGroupAsync(string groupName);

        Task SendPublicMessage(string message);
        Task SendMessageToGroup(string groupName, string message);
    }
}
