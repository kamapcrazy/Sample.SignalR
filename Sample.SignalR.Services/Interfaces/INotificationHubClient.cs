using System.Threading.Tasks;

namespace Sample.SignalR.Services.Interfaces
{
    public interface INotificationHubClient
    {
        Task OnConnected(string message);

        Task OnPublicMessageSent(string message);

        Task OnPrivateMessageSent(string message);
    }
}
