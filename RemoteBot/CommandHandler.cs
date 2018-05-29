using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteBot
{
    public class CommandHandler
    {
        public string HandleCommand(string cmd)
        {
            var showCommands = false;
            var parts = cmd.Split(' ');
            switch (parts[0]?.ToLower() ?? "")
            {
                case "hello":
                case "hey":
                    return $"{cmd}, How can I help you? Remember that you can type 'help' for more options.";
                case "actions":
                case "options":
                case "commands":
                case "help":
                    return GetCommands();
                default:
                    return $"I am not sure what you mean by '{cmd}'?{Environment.NewLine}{GetCommands()}";
            }

        }

        private string GetCommands()
        {
            return $"Commands:" +
                $"{Environment.NewLine} ACTION1" +
                $"{Environment.NewLine} ACTION2";
        }
    }
}
