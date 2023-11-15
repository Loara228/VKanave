using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.DB;
using VKanave.Networking.NetObjects;
using VKanaveServer.Core;

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
            this.Chats = chats;
            count = Chats.Length;
            TokenRequired = true;
        }

        public override void Action(Connection from)
        {
            string query = 
                $"SELECT * FROM `messages` WHERE (`id_from` = {localUserId} or `id_to` = {localUserId}) and (`id_from` = {userId2} or `id_to` = {userId2}) order by `id` DESC limit 10;";

            base.Action(from);

            Exception exc = Database.RunCommand(query, out var table);
            if (exc == null)
            {
                List<ChatMessage> messages = new List<ChatMessage>();
                foreach (var row in table.rows)
                {
                    int id = int.Parse(row.values[0].ToString());
                    int id_from = int.Parse(row.values[1].ToString());
                    int id_to = int.Parse(row.values[2].ToString());
                    int date = int.Parse(row.values[3].ToString());
                    string content = row.values[4].ToString();
                    int flags = int.Parse(row.values[5].ToString());
                    if (id_from == localUserId)
                    {
                        flags = (int)((ChatMessageFlags)flags | ChatMessageFlags.OUTBOX);
                    }
                    messages.Add(
                        new ChatMessage(new ChatUser(0, "", 0),
                        id,
                        content,
                        date,
                        flags
                        ));
                }
                from.Send(new NMLoadMessages(messages.ToArray()));
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

        public int count;
        public long localUserId;
        public long userId2;
    }
}
