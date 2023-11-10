using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanaveServer
{
    internal enum LogType
    {
        System,
        Information,
        Serialization,
        SerializationLow,
        Networking,
        NetworkingLow,
        Connection,
        SQL
    }
}
