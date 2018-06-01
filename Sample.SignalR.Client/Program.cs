using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sample.SignalR.Client.Implementations;
using Sample.SignalR.Client.Interfaces;

namespace Sample.SignalR.Client
{
    class Program
    {
        private readonly ISignalRClient _signalRClient;

        public Program(ISignalRClient signalRClient, ILoggerProvider loggerProvider)
        {
            _signalRClient = new NotificationSignalRClient(new Uri("http://localhost:56479/signalr/notification"), loggerProvider);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("hello world");

            Console.ReadLine();
        }


    }
}
