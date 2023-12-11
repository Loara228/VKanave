using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.Models;
using VKanave.Views;

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

            if (ChatPage.Current != null && ChatPage.Current.User != null)
            {
                MessageModel msg2 = null;
                foreach (var chatMsg in ChatPage.Current.Messages)
                {
                    if (chatMsg.ID == messageId)
                    {
                        msg2 = chatMsg;
                        break;
                    }
                }
                if (msg2 != null)
                {
                    msg2.Delete();
                }
            }
            else
                LoadMessages();
        }

        private void LoadMessages() => Networking.Send(new NMChats() { localUserId = LocalUser.Id });

        public long dialogId;
        public long messageId;

    }
}
