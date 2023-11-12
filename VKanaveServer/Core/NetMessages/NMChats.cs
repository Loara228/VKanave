using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VKanave.DB;
using VKanave.Networking.NetObjects;
using VKanaveServer;
using VKanaveServer.Core;

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
            string query = $"SELECT messages.id,messages.id_from, messages.id_to, messages.date, messages.content, messages.flags, MAX(`date`), users.username " +
                $"FROM `messages`,`users` " +
                $"WHERE `id_from`={localUserId} OR `id_to`={localUserId} GROUP BY CONCAT(LEAST(`id_from`,`id_to`),'-',GREATEST(`id_from`,`id_to`)) " +
                $"limit 10;";

            base.Action(from);
            Exception exc = Database.RunCommand(query, out var table);
            if (exc == null)
            {
                List<ChatMessage> messages = new List<ChatMessage>();
                foreach(var row in table.rows)
                {
                    Console.WriteLine(row.values.Count);
                    long messageId = long.Parse(row.values[0].ToString());
                    long user1 = long.Parse(row.values[1].ToString());
                    long user2 = long.Parse(row.values[2].ToString());
                    long user = user1 == localUserId ? user1 : user2;
                    int date = int.Parse(row.values[3].ToString());
                    string content = row.values[4].ToString();
                    int flags = int.Parse(row.values[5].ToString());
                    string username = row.values[7].ToString();
                    messages.Add(
                        new ChatMessage(
                                new ChatUser(
                                    user,
                                    username),
                            messageId,
                            content,
                            date,
                            flags));
                    from.Send(new NMChats(messages.ToArray()));
                }
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

    }
}
