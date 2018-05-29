using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using ReverseConnectionBot.Hubs;

namespace ReverseConnectionBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            BotHub.SetConnectionNotificationCallback(x =>
            {
                var t = context.PostAsync(x);
            });
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var msg = context.MakeMessage();
            msg.Type = ActivityTypes.Typing;
            await context.PostAsync(msg);

            //var showCommands = false;

            var activity = await result as Activity;
            BotHub.SendCommand(activity.Text, x =>
            {
                var t = context.PostAsync(x);
            });

            context.Wait(MessageReceivedAsync);

            /*
             * var parts = activity.Text.Split(' ');
            switch (parts[0]?.ToLower() ?? "")
            {
                case "hello":
                case "hey":
                    await context.PostAsync($"{activity.Text} {activity.From.Name}");
                    await context.PostAsync($"How can I help you? Remember that you can type 'help' for more options.");
                    break;
                case "actions":
                case "options":
                case "commands":
                case "help":
                    showCommands = true;
                    break;
                default:
                    await context.PostAsync($"{activity.From.Name} I am not sure what you mean by '{activity.Text}'?");
                    showCommands = true;
                    break;
            }

            if (showCommands)
                await context.PostAsync($"Commands:" +
                        $"{Environment.NewLine} ACTION1" +
                        $"{Environment.NewLine} ACTION2");

            context.Wait(MessageReceivedAsync);
            */
        }
    }
}