using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.DB;
using VKanave.Networking.NetObjects;
using VKanaveServer;
using VKanaveServer.Core;

namespace VKanave.Networking.NetMessages
{
    public class NMNewChat : NMAction
    {
        public NMNewChat()
        {
            TokenRequired = true;
        }

        public override void Action(Connection from)
        {
            base.Action(from);

            Database.RunCommand($"SELECT user_id FROM `users` WHERE username = \"{username}\"", out var table);
            if (table.rows.Count > 0)
            {
                long id1 = from.User.ID;
                long id2 = long.Parse(table.rows[0].values[0].ToString());
                if (id1 == id2)
                {
                    from.Send(new NMNewChat() { result = (int)NewChatResult.USER_NOT_FOUND });
                    return;
                }
                Database.RunCommand($"SELECT id FROM `messages` WHERE (id_from = {id1} and id_to = {id2}) or (id_from = {id2} and id_to = {id1})", out table);
                if (table.rows.Count > 0)
                {
                    from.Send(new NMNewChat() { result = (int)NewChatResult.ALREADY_EXISTS });
                    return;
                }
                CreateChat(id1, id2, from);
                from.Send(new NMNewChat() { result = (int)NewChatResult.SUCCESS });
                return;
            }
            else
            {
                from.Send(new NMNewChat() { result = (int)NewChatResult.USER_NOT_FOUND });
            }
        }

        private void CreateChat(long user1, long user2, Connection from)
        {
            NMChatMessage chatMsg = new NMChatMessage()
            {
                ChatMessage =
                new ChatMessage(
                    new ChatUser(0, "", 0, ""),
                    0,
                    "Chat created",
                    Database.GetUnixTime(),
                    (int)ChatMessageFlags.SYSTEM)
            };
            chatMsg.TokenRequired = false;
            chatMsg.id_from = user1;
            chatMsg.id_to = user2;
            chatMsg.SystemMessage = true;
            chatMsg.Action(from);
        }

        public string username = string.Empty;

        /// <summary>
        /// enum NetChatResult
        /// </summary>
        public int result;

    }
}
