using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanaveServer.Core.NetMessages
{

    [Flags]
    public enum ChatMessageFlags : int
    {
        NONE = 0,
        UNREAD = 1,
        OUTBOX = 2,
        DELETED = 4
    }
}
