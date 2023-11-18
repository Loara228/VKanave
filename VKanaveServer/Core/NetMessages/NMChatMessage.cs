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
    public class NMChatMessage : NMAction
    {
        public NMChatMessage()
        {
            TokenRequired = true;
        }

        public override void Action(Connection from)
        {
            base.Action(from);
            Exception exc = Database.RunCommand($"INSERT INTO `messages` (`id_from`, `id_to`, `date`, `content`, `flags`) VALUES ('{id_from}', '{id_to}', '{Database.GetUnixTime()}', '{ChatMessage.Content}', '64');", out var tb1);
            bool read = Server.Current?.GetConnectionFromUserId(id_to) != null;
            if (read)
                this.ChatMessage.Flags = (int)ChatMessageFlags.OUTBOX;
            else
                this.ChatMessage.Flags = (int)(ChatMessageFlags.OUTBOX | ChatMessageFlags.UNREAD);
            from.Send(new NMChatMessage()
            {
                ChatMessage = this.ChatMessage,
                id_to = this.id_to,
                id_from = this.id_from
            });
            if (read)
                this.ChatMessage.Flags = (int)(ChatMessageFlags.UNREAD);
            else
                this.ChatMessage.Flags = 0;
            Connection? connectionTo = Server.Current?.GetConnectionFromUserId(id_to);
            if (connectionTo != null)
            {
                connectionTo.Send(new NMChatMessage()
                {
                    ChatMessage = this.ChatMessage,
                    id_to = this.id_to,
                    id_from = this.id_from
                });
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
            get; private set;
        }

        public long id_to, id_from;

    }
}
