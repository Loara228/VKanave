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
            string query = $"SELECT messages.id_from, messages.id_to, MAX(`date`) " +
                $"FROM `messages`,`users` " +
                $"WHERE `id_from`={localUserId} OR `id_to`={localUserId} GROUP BY CONCAT(LEAST(`id_from`,`id_to`),'-',GREATEST(`id_from`,`id_to`)) " +
                $"limit 10;";
            base.Action(from);
            Exception exc = Database.RunCommand(query, out var table);
            if (exc == null)
            {
                List<ChatMessage> messages = new List<ChatMessage>();
                foreach (var row in table.rows)
                {
                    long id_from = long.Parse(row.values[0].ToString());
                    long id_to = long.Parse(row.values[1].ToString());
                    int date = int.Parse(row.values[2].ToString());
                    messages.Add(LoadLastMessage(id_from, id_to, date));
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

        private ChatMessage LoadLastMessage(long user_from, long user_to, int max_date)
        {
            long secondUser = user_from != localUserId ? user_from : user_to;
            string queryJoin = $"join users on users.user_id = {secondUser} ";   
            string query = 
                $"select id, date, content, flags, users.user_id, users.username, users.last_active, users.display_name " +
                $"from messages " +
                queryJoin +
                $"where ((id_to = {localUserId} and id_from = {secondUser}) or (id_to = {secondUser} and id_from = {localUserId})) and `date` = {max_date}";
            Database.RunCommand(query, out SQLTable table);

            long messageId = long.Parse(table.rows[0].values[0].ToString());
            int messageDate = int.Parse(table.rows[0].values[1].ToString());
            string messageContent = table.rows[0].values[2].ToString();
            int messageFlags = int.Parse(table.rows[0].values[3].ToString());

            if (localUserId == user_from)
                messageFlags += (int)ChatMessageFlags.OUTBOX;
            if (((ChatMessageFlags)messageFlags).HasFlag(ChatMessageFlags.DELETED))
                messageContent = "MESSAGE DELETED";

            long userId = long.Parse(table.rows[0].values[4].ToString());
            string userUsername = table.rows[0].values[5].ToString();
            int userLastActive = int.Parse(table.rows[0].values[6].ToString());
            string displayName = table.rows[0].values[7] == null ? "" : table.rows[0].values[7].ToString();

            return new ChatMessage(
                new ChatUser(secondUser, userUsername, userLastActive, displayName),
                messageId,
                messageContent,
                messageDate,
                messageFlags);
        }

        public ChatMessage[] Chats
        {
            get; set;
        }

        public int count;
        public long localUserId;

    }
}
