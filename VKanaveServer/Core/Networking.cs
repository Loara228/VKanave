using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;  
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using VKanave.Networking.NetMessages;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VKanaveServer.Core
{
    internal static class Networking
    {
        internal static void ReceiveData(Connection connection)
        {
            NetworkStream stream = connection.Stream;

            byte[] data = new byte[1024];
            stream.Read(data, 0, data.Length);

            if (data[0] == 0 && data[1] == 0 && data[2] == 0)
                return;

            Program.Log(LogType.Networking, $"data received. {data.Length} bytes ({connection.Index})");
            NetMessage msg = NetMessage.Create(data);
            msg.Deserialize();
            Program.Log(LogType.Serialization, $"{msg} deserialized ({connection.Index})");
            PrcMsg(connection, msg);
        }

        internal static void Send(Connection connection, NetMessage msg)
        {
            msg.Serialize();
            Program.Log(LogType.Serialization, $"{msg} serialized ({connection.Index})");
            connection.Stream.Write(msg.Buffer);
            Program.Log(LogType.Networking, $"data sent. {msg.Buffer.Length} bytes ({connection.Index})");
        }

        internal static void PrcMsg(Connection from, NetMessage msg)
        {
            if (msg is NMAction)
                (msg as NMAction).Action(from);
        }
    }
}
