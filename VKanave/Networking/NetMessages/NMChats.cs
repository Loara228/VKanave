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
            lock(_block)
            {
                ChatsPage.Chats.Clear();
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ChatsPage.Current.activityIndicator.IsRunning = false;
                    ChatsPage.Current.activityIndicator.IsVisible = false;
                    if (Chats != null && Chats.Length > 0)
                        Chats.ToList().ForEach(x =>
                        {
                            ChatsPage.Chats.Add(new ChatModel(new UserModel(x.User.Username, x.User.ID, x.Date, x.User.DisplayName),
                                                    new MessageModel(0, x.Content, x.Date, (ChatMessageFlags)x.Flags)));
                        });
                    else
                    {
                        // no messages
                    }
                });
            }
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

        private static readonly object  _block = new object();

        public int count;
        public long localUserId;
    }
}
