using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.Models;
using VKanave.Networking.NetMessages.NMCMFlags;
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
            if (ChatPage.Current != null && ChatPage.Current.User != null)
            {
                if (ChatPage.Current.User.ID == id_from || ChatPage.Current.User.ID == id_to)
                {
                    var flags = (ChatMessageFlags)ChatMessage.Flags;
                    if (ChatPage.Current.User.ID == id_from)
                    {
                        flags = (ChatMessageFlags)ChatMessage.Flags;
                        flags &= ~ChatMessageFlags.UNREAD;
                    }
                    ChatPage.Current.Messages.Add(new MessageModel(ChatMessage.ID, ChatMessage.Content, ChatMessage.Date, flags));
                    if (!((ChatMessageFlags)ChatMessage.Flags).HasFlag(ChatMessageFlags.OUTBOX))
                        Networking.Send(new NMRead() { messageId = ChatMessage.ID, chatId = ChatPage.Current.User.ID, userId2 = LocalUser.Id });
                }
            }
            else
            {
                Networking.Send(new NMChats() { localUserId = LocalUser.Id });
            }
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
