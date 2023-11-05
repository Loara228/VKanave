using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.Networking.NetMessages
{
    [Serializable]
    public abstract class NMAction : NetMessage
    {
        public abstract void Action(Connection from);
    }
}
