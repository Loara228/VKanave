using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.Networking.NetObjects
{
    [Flags]
    public enum ChatMessageFlags : int
    {
        NONE = 0,
        UNREAD = 1,
        OUTBOX = 2,
        DELETED = 4,
    }
}
