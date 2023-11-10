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

    [Serializable]
    public class NMAuth : NMAction
    {
        public NMAuth()
        {
            username = string.Empty;
            password = string.Empty;
            token = string.Empty;
        }

        public override void Action(Connection from)
        {
            Exception exc = Database.RunCommand($"SELECT `token` FROM `users` WHERE `username`='{username}' and `password_hash`='{password}'", out SQLTable table);
            if (exc != null)
                return;
            if (table.rows.Count > 0)
            {
                string? tokendb = table.rows[0].values[0].ToString();
                Program.Log(LogType.Information, tokendb);
                if (tokendb != null)
                {
                    NMAuth msg = new NMAuth() { token = tokendb };
                    msg.username = this.username;
                    from.Send(msg);
                    return;
                }
            }
            from.Send(new NMAuth() { token = "" });
        }

        public string username, password, token;

    }
}
