using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.Networking
{
    [Flags]
    public enum ChatMessageFlags : int
    {
        UNREAD,
        OUTBOX,
        DELETED
    }
}
