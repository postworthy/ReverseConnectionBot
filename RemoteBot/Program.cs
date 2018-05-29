using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var secret = ConfigurationManager.AppSettings["Secret"];
            var endpoint = ConfigurationManager.AppSettings["SignalrEndpoint"];

            var connection = new HubConnection(endpoint);
            var botHub = connection.CreateHubProxy("BotHub");

            Console.WriteLine("Connecting Bot");

            connection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                    Console.WriteLine("There was an error opening the connection:{0}", task.Exception.GetBaseException());
                else
                    Console.WriteLine("Bot Connected");
            }).Wait();

            Console.WriteLine("Sending Verification");

            botHub.Invoke<string>("VerifyBotSecret", secret).ContinueWith(task =>
            {
                if (task.IsFaulted)
                    Console.WriteLine("There was an error calling VerifyBotSecret: {0}", task.Exception.GetBaseException());
                else
                    Console.WriteLine("Verification Successful");
            });

            botHub.On<string, string>("handleCommand", (actionID, cmd) =>
            {
                Console.WriteLine("New Command: {0}", cmd);

                var cmdHandler = new CommandHandler();

                botHub.Invoke<string>("CommandResponse", actionID, cmdHandler.HandleCommand(cmd)).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                        Console.WriteLine("There was an error calling CommandResponse: {0}", task.Exception.GetBaseException());
                    else
                        Console.WriteLine("Command Handled");
                });

            });

            Console.WriteLine("Press Any Key To Exit");
            Console.Read();
        }

    }
}
