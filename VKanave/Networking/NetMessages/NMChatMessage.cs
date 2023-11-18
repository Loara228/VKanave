using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.Models;
using VKanave.Networking.NetObjects;
using VKanave.Views;

namespace VKanave.Networking.NetMessages
{
    public class NMChatMessage : NMAction
    {
        public NMChatMessage()
        {
            TokenRequired = true;
        }

        public override void Action(Connection from)
        {
            base.Action(from);
            if (ChatPage.Current != null)
            {
                if (ChatPage.Current.User.ID == id_from || ChatPage.Current.User.ID == id_to)
                {
                    ChatPage.Current.Messages.Add(new MessageModel(0, ChatMessage.Content, ChatMessage.Date, (ChatMessageFlags)ChatMessage.Flags));
                }
            }
            else
            {
            }
            Networking.Send(new NMChats() { localUserId = LocalUser.Id });
        }

        protected override void OnSerialize()
        {
            Write(ChatMessage);
        }

        protected override void OnDeserialize()
        {
            ChatMessage = ReadChatMessage();
        }

        public ChatMessage ChatMessage
        {
            get; set;
        }

        public long id_to, id_from;

    }
}
