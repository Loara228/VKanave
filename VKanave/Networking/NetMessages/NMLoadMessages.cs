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
    public class NMLoadMessages : NMAction
    {
        public NMLoadMessages()
        {
            TokenRequired = true;
        }

        public NMLoadMessages(ChatMessage[] chats)
        {
            this.Messages = chats;
            count = Messages.Length;
            TokenRequired = true;
        }

        public override void Action(Connection from)
        {
            ChatPage.Current.Messages.Clear();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (Messages != null && Messages.Length > 0)
                {
                    Networking.Send(new NMReadAll() { chatId = ChatPage.Current.User.ID, userId2 = LocalUser.Id });
                    var messages = Messages.ToList();
                    messages.Reverse();
                    Messages = messages.ToArray();
                    Messages.ToList().ForEach(x =>
                    {
                        ChatPage.Current.Messages.Add(new MessageModel(x.ID, x.Content, x.Date, (ChatMessageFlags)x.Flags));
                    });
                }
                else
                {
                    // нету сообщений
                }
            });
        }

        protected override void OnSerialize()
        {
            if (count > 0)
                Messages.ToList().ForEach(Write);
        }

        protected override void OnDeserialize()
        {
            if (count == 0)
                return;
            List<ChatMessage> chatsList = new List<ChatMessage>();
            for (int i = 0; i < count; i++)
            {
                chatsList.Add(ReadChatMessage());
            }
            Messages = chatsList.ToArray();
        }

        public ChatMessage[] Messages
        {
            get; set;
        }

        public int count;
        public long localUserId;
        public long userId2;
    }
}
