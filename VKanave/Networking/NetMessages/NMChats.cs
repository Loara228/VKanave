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
    public class NMChats : NMAction
    {
        public NMChats()
        {
            TokenRequired = true;
        }

        public NMChats(ChatMessage[] chats)
        {
            this.Chats = chats;
            count = Chats.Length;
            TokenRequired = true;
        }

        public override void Action(Connection from)
        {
            base.Action(from);
            ChatsPage.Chats.Clear();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (Chats != null && Chats.Length > 0)
                    Chats.ToList().ForEach(x =>
                    {
                        ChatsPage.Chats.Add(new ChatModel(new UserModel(x.User.Username, x.User.User),
                                                new MessageModel(0, x.Content, x.Date, (ChatMessageFlags)x.Flags)));
                    });
                else
                {
                    ChatsPage.Current.label1.IsVisible = true;
                    ChatsPage.Current.button1.IsVisible = true;
                }
            });
        }

        protected override void OnSerialize()
        {
            if (count > 0)
                Chats.ToList().ForEach(Write);
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
            Chats = chatsList.ToArray();
        }

        public ChatMessage[] Chats
        {
            get; set;
        }

        public int count;
        public long localUserId;
    }
}
