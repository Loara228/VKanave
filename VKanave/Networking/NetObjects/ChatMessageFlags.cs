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
        OUTBOX = 1,
        DELETED = 2,
        UNREAD = 64,
    }
}
