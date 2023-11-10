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
        UNREAD,
        OUTBOX,
        DELETED
    }
}
