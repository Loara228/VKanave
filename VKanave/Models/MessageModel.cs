using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.Extensions;
using VKanave.Networking.NetObjects;

namespace VKanave.Models
{
    public record class MessageModel(long ID, string Content, long UnixTime, ChatMessageFlags Flags)
    {
        public DateTime DateTime
        {
            get
            {
                return UnixTime.ToDateTime();
            }
        }

        public string Preview
        {
            get
            {
                if (Content.Length > 30)
                    return Content.Substring(0, 25) + "...";
                return Content;
            }
        }

        public bool Unread
        {
            get => Flags == ChatMessageFlags.UNREAD;
        }
    }
}
