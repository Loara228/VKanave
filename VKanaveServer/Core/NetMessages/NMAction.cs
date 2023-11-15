using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.DB;
using VKanaveServer;
using VKanaveServer.Core;

namespace VKanave.Networking.NetMessages
{
    public abstract class NMAction : NetMessage
    {
        public virtual void Action(Connection from)
        {
            if (TokenRequired)
            {
                if (from.Token != _token)
                {
                    from.Disconnect();
                    Program.Log(LogType.Information, $"received invalid token.");
                }
                else
                {
                    _ = Database.RunCommand($"UPDATE `users` SET `last_active`= {Database.GetUnixTime()} WHERE `token` = '{_token}'", out var _);
                }
            }
        }

        public bool TokenRequired
        {
            get; set;
        } = false;

        public string _token = string.Empty;
    }
}
