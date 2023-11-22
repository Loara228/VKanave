using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.Models;
using VKanave.Views;

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
            if (ChatPage.Current != null && ChatPage.Current.User != null)
            {
                foreach (var chatMsg in ChatPage.Current.Messages)
                {
                    if (chatMsg.Local && chatMsg.ID == messageId)
                    {
                        chatMsg.Unread = false;
                        return;
                    }
                }
            }
            else
                LoadMessages();
        }

        private void LoadMessages() => Networking.Send(new NMChats() { localUserId = LocalUser.Id });

        public long chatId, userId2;
        public long messageId;

    }
}
