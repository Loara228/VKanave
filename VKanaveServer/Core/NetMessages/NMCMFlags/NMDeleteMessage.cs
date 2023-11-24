using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.DB;
using VKanave.Networking.NetObjects;
using VKanaveServer.Core;


namespace VKanave.Networking.NetMessages.NMCMFlags
{
    public class NMDelete : NMAction
    {
        public NMDelete()
        {
            TokenRequired = true;
        }

        public override void Action(Connection from)
        {
            base.Action(from);

            Database.RunCommand($"update `messages` set `flags` = {(int)ChatMessageFlags.DELETED} WHERE id = {messageId}", out _);

            Connection? connection = Server.Current?.GetConnectionFromUserId(dialogId);
            connection?.Send(new NMDelete() { dialogId = this.dialogId, messageId = this.messageId });
        }

        public long dialogId;
        public long messageId;

    }
}
