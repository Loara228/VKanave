using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.Networking.NetObjects
{
    public enum NewChatResult : int
    {
        UNKNOWN = 0,
        SUCCESS = 1,
        ALREADY_EXISTS = 2,
        USER_NOT_FOUND = 3
    }
}
