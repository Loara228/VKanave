using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.Views;

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
            if (ChatPage.Current != null && ChatPage.Current.User != null)
            {
                if (ChatPage.Current.User.ID == chatId || ChatPage.Current.User.ID == userId2)
                {
                    var chatMessages = ChatPage.Current.Messages.Where(x => x.Local && x.Unread).ToList();
                    chatMessages.ForEach(x =>
                    {
                        x.Unread = false;
                    });
                }
            }
            else
                LoadMessages();
        }

        private void LoadMessages() => Networking.Send(new NMChats() { localUserId = LocalUser.Id });

        public long chatId, userId2;
    }
}
