using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.Networking.NetObjects
{
    public class ChatMessage
    {
        public ChatMessage(ChatUser user, long id, string content, int date, int flags)
        {
            this.User = user;
            this.ID = id;
            this.Content = content;
            this.Date = date;
            this.Flags = flags;
        }

        public ChatUser User
        {
            get; private set;
        }

        public long ID
        {
            get; private set;
        }

        public string Content
        {
            get; private set;
        } = "";

        public int Date
        {
            get; private set;
        }

        public int Flags
        {
            get; set;
        }
    }
}
