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
    public class NMRead : NMAction
    {
        public NMRead()
        {
            TokenRequired = true;
        }

        public override void Action(Connection from)
        {
            base.Action(from);

            Database.RunCommand($"update `messages` set `flags` = `flags` - 64 WHERE id = {messageId} and `flags` > 63", out _);

            Program.Log(LogType.Console, $"user with id {from.User.ID} has read message with id {messageId}");

            //from.Send(new NMRead() { chatId = this.chatId, messageId = this.messageId, userId2 = this.userId2 });

            Connection? connection = Server.Current?.GetConnectionFromUserId(chatId);
            if (connection != null)
            {
                connection.Send(new NMRead() { chatId = this.chatId, messageId = this.messageId, userId2 = this.userId2 });
            }
        }

        public long chatId, userId2;
        public long messageId;

    }
}
