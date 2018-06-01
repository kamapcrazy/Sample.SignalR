using System.Threading.Tasks;

namespace Sample.SignalR.Client.Interfaces
{
    public interface ISignalRClient
    {
        Task StartSignalRConnectionAsync();
    }
}
