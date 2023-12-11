using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.Networking.NetMessages
{
    public class NMSetBio : NMAction
    {
        public NMSetBio()
        {
            TokenRequired = true;
        }

        public override void Action(Connection from)
        {
            base.Action(from);
        }

        public long userId = 0;
        public string bio = string.Empty;

    }
}
