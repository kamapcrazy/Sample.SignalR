using System.Threading.Tasks;

namespace Sample.SignalR.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendPublicMessage(string message);
        Task SendMessageToGroup(string message, string groupName = "");
    }
}
