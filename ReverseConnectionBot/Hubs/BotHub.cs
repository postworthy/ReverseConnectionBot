using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ReverseConnectionBot.Hubs
{
    public class BotHub : Hub
    {
        private static string secret = "F0C03E98-CC8B-4078-827F-052AB2C7CADE";
        private static List<string> reverseBotConnectionIDs = new List<string>();
        private static Dictionary<string, Action<string>> pendingActions = new Dictionary<string, Action<string>>();
        private static Action<string> RaiseConnectionNotification = new Action<string>(_ => { });

        public static void SetConnectionNotificationCallback(Action<string> callback)
        {
            if (callback != null)
                RaiseConnectionNotification = callback;
            else
                RaiseConnectionNotification = new Action<string>(_ => { });
        }

        public static void SendCommand(string cmd, Action<string> handleResponse)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<BotHub>();
            var ids = GetReverseBotConnectionIDs();

            var actionID = Guid.NewGuid().ToString();

            pendingActions.Add(actionID, handleResponse);

            context.Clients.Clients(ids).handleCommand(actionID, cmd);
        }

        public void CommandResponse(string actionID, string response)
        {
            var action = pendingActions[actionID];
            if (action != null)
            {
                pendingActions.Remove(actionID);
                action(response);
            }
        }

        public void VerifyBotSecret(string secret)
        {
            if (secret == BotHub.secret)
                reverseBotConnectionIDs.Add(Context.ConnectionId);
        }

        private static List<string> GetReverseBotConnectionIDs()
        {
            return reverseBotConnectionIDs;
        }

        public override Task OnConnected()
        {
            RaiseConnectionNotification($"Connected {Context.ConnectionId}");
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            RaiseConnectionNotification($"Disconnected {Context.ConnectionId}");
            return base.OnDisconnected(stopCalled);
        }
        public override Task OnReconnected()
        {
            RaiseConnectionNotification($"Reconnected {Context.ConnectionId}");
            return base.OnReconnected();
        }
    }
}