using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sample.SignalR.Client.Interfaces
{
    public interface ISignalRServer
    {
        Task SendMessageAsync(string message);
    }
}
