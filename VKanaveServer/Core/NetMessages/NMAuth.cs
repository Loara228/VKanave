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
            Exception exc = Database.RunCommand($"SELECT `token`,`user_id` FROM `users` WHERE `username`='{username}' and `password_hash`='{password}'", out SQLTable table);
            if (exc != null)
                return;
            if (table.rows.Count > 0)
            {
                string? tokendb = table.rows[0].values[0].ToString();
                Program.Log(LogType.Information, tokendb);
                if (tokendb != null)
                {
                    from.Token = tokendb;
                    long locid = long.Parse(table.rows[0].values[1].ToString());
                    NMAuth msg = new NMAuth() { token = tokendb, id = locid };
                    msg.username = this.username;
                    _ = Database.RunCommand($"UPDATE `users` SET `last_active`= {Database.GetUnixTime()} WHERE `user_id` = {locid}", out var _);
                    from.User = new NetObjects.ChatUser(locid, username, 0);
                    from.Send(msg);
                    return;
                }
            }
            from.Send(new NMAuth() { token = ""});
        }

        public string username, password, token;
        public long id;

    }
}
