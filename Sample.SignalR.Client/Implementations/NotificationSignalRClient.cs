using Microsoft.Extensions.Logging;
using Sample.SignalR.Client.Interfaces;
using System;

namespace Sample.SignalR.Client.Implementations
{
    public class NotificationSignalRClient : BaseClient
    {
        private readonly HubProxy<ISignalRServer, ISignalRClient> _hubProxy;

        public NotificationSignalRClient(Uri signalRUri, ILoggerProvider loggerProvider)
            : base(signalRUri, loggerProvider)
        {
            _hubProxy = new HubProxy<ISignalRServer, ISignalRClient>(SignalRConnection);

        }
    }
}
