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
            //string query = $"SELECT messages.id,messages.id_from, messages.id_to, messages.date, messages.content, messages.flags, MAX(`date`), users.username, users.last_active " +
            //    $"FROM `messages`,`users` " +
            //    $"WHERE `id_from`={localUserId} OR `id_to`={localUserId} GROUP BY CONCAT(LEAST(`id_from`,`id_to`),'-',GREATEST(`id_from`,`id_to`)) " +
            //    $"limit 10;";
            string query = $"SELECT messages.id,messages.id_from, messages.id_to, messages.date, messages.content, messages.flags, MAX(`date`), users.username, users.last_active " +
                $"FROM `messages`,`users` " +
                $"WHERE `id_from`={localUserId} OR `id_to`={localUserId} GROUP BY CONCAT(LEAST(`id_from`,`id_to`),'-',GREATEST(`id_from`,`id_to`)) " +
                $"limit 10;";



            base.Action(from);
            Exception exc = Database.RunCommand(query, out var table);
            if (exc == null)
            {
                ChatUser chatUser = null;
                List<ChatMessage> messages = new List<ChatMessage>();
                foreach (var row in table.rows)
                {
                    long messageId = long.Parse(row.values[0].ToString());
                    long user1 = long.Parse(row.values[1].ToString());
                    long user2 = long.Parse(row.values[2].ToString());
                    long user = user1 != localUserId ? user1 : user2;
                    int date = int.Parse(row.values[3].ToString());
                    string content = row.values[4].ToString();
                    int flags = int.Parse(row.values[5].ToString());
                    string username = row.values[7].ToString();

                    // userinfo
                    Database.RunCommand($"SELECT users.username, users.last_active FROM `users` WHERE `user_id`={user2}", out var tb1);


                    if (user1 == localUserId)
                    {
                        chatUser = new ChatUser(user2, tb1.rows[0].values[0].ToString(), int.Parse(tb1.rows[0].values[1].ToString()));
                    }
                    else
                    {
                        chatUser = new ChatUser(user, username, int.Parse(tb1.rows[0].values[1].ToString()));
                    }
                    messages.Add(
                        new ChatMessage(
                                chatUser,
                            messageId,
                            content,
                            date,
                            flags));
                }
                from.Send(new NMChats(messages.ToArray()));
            }
            else
            {
                from.Send(new NMChats());
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
