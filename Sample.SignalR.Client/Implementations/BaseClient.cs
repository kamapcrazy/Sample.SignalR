using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Sample.SignalR.Client.Interfaces;

namespace Sample.SignalR.Client.Implementations
{
    public abstract class BaseClient : ISignalRClient
    {
        private const int ReconnectIntervalMs = 5000;

        private readonly Uri _signalRUri;

        protected readonly HubConnection SignalRConnection;

        protected BaseClient(Uri signalRUri, ILoggerProvider loggerProvider)
        {
            _signalRUri = signalRUri;

            SignalRConnection = new HubConnectionBuilder()
                .WithUrl(_signalRUri.OriginalString)
                .ConfigureLogging(loggingBuilder => loggingBuilder.AddProvider(loggerProvider))
                .Build();

            SignalRConnection.Closed += OnSignalRClosed;
        }

        public event EventHandler<string> SignalRError;
        public event EventHandler<string> SignalRDisconnected;

        public async Task StartSignalRConnectionAsync()
        {
            while (true)
            {
                try
                {
                    await SignalRConnection.StartAsync();
                    break;
                }
                catch (Exception ex)
                {
                    OnSignalRError(ex.Message);
                    await Task.Delay(ReconnectIntervalMs);
                }
            }
        }

        private async Task OnSignalRClosed(Exception exception)
        {
            OnSignalRDisconnected(_signalRUri.OriginalString);
            await Task.Delay(ReconnectIntervalMs);
            await StartSignalRConnectionAsync();
        }

        protected virtual void OnSignalRError(string e)
        {
            SignalRError?.Invoke(this, e);
        }

        protected virtual void OnSignalRDisconnected(string e)
        {
            SignalRDisconnected?.Invoke(this, e);
        }
    }
}
