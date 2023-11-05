using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanaveServer.Core;

namespace VKanave.Networking.NetMessages
{

    [Serializable]
    public class NMAuth : NMAction
    {
        public NMAuth()
        {
            username = string.Empty;
            password = string.Empty;
            token = string.Empty;
        }

        public override void Action(Connection from)
        {
            from.Send(new NMAuth() { token = "H2A4GSD32KJA2G2F4KAJ23DF" });
        }

        public string username, password, token;

    }
}
