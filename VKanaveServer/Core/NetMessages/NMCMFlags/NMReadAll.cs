using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.DB;
using VKanaveServer;
using VKanaveServer.Core;


namespace VKanave.Networking.NetMessages.NMCMFlags
{
    public class NMReadAll : NMAction
    {
        public NMReadAll()
        {
            TokenRequired = true;
        }

        public override void Action(Connection from)
        {
            base.Action(from);
            Program.Log(LogType.Console, $"user with id {from.User.ID} has read all the messages");
            Connection? connection = Server.Current?.GetConnectionFromUserId(chatId);
            if (connection != null)
            {
                connection.Send(new NMReadAll() { chatId = this.chatId, userId2 = this.userId2 });
            }
        }

        public long chatId, userId2;
    }
}
