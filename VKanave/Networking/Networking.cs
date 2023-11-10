using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using VKanave.Networking.NetMessages;

namespace VKanave.Networking
{
    internal class Networking
    {
        internal static void ReceiveData()
        {
            NetworkStream stream = Connection.Current.Stream;

            byte[] data = new byte[1024];
            stream.Read(data, 0, data.Length);

            if (data[0] == 0 && data[1] == 0 && data[2] == 0)
                return;

            NetMessage msg = NetMessage.Create(data);
            msg.Deserialize();
            PrcMsg(Connection.Current, msg);
        }

        internal static void Send(NetMessage msg)
        {
            msg.Serialize();
            byte[] data = msg.Buffer;
            Connection.Current.Stream.Write(data, 0, data.Length);
        }

        internal static void PrcMsg(Connection from, NetMessage msg)
        {
            if (msg is NMAction)
                (msg as NMAction).Action(from);
        }
    }
}
