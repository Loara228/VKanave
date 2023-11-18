using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Data;
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

            byte[] data = new byte[NetMessage.BUFFER_SIZE];
            bool emptyBuffer = true;
            while (true)
            {
                byte[] receivedData = new byte[NetMessage.BUFFER_SIZE];
                stream.Read(receivedData, 0, receivedData.Length);

                if (IsBufferEmpty(receivedData))
                {
                    data = data.Concat(receivedData).ToArray();
                    CheckConnection(connection);
                    break;
                }
                else
                {
                    connection.EmptyBuffersCount = 0;
                    if (!emptyBuffer)
                    {
                        data = data.Concat(receivedData).ToArray();
                    }
                    else
                        data = receivedData;
                    emptyBuffer = false;
                }
            }

            if (emptyBuffer)
            {
                Program.Log(LogType.Networking, $"Empty buffer received. ({connection.Index})");
                return;
            }

            Program.Log(LogType.SrlzLow, string.Join(' ', data));

            Program.Log(LogType.Networking, $"data received. {data.Length} bytes ({connection.Index})");
            NetMessage msg = NetMessage.Create(data);
            msg.Deserialize();
            Program.Log(LogType.SrlzHight, $"{msg} deserialized ({connection.Index})");
            PrcMsg(connection, msg);
        }

        internal static void Send(Connection connection, NetMessage msg)
        {
            msg.Serialize();
            Program.Log(LogType.SrlzHight, $"{msg} serialized ({connection.Index})");
            lock (connection.block)
                connection.Stream.Write(msg.Buffer);
            Program.Log(LogType.Networking, $"data sent. {msg.Buffer.Length} bytes ({connection.Index})");
        }

        internal static void PrcMsg(Connection from, NetMessage msg)
        {
            if (msg is NMAction)
                (msg as NMAction).Action(from);
        }

        private static bool IsBufferEmpty(byte[] buffer)
        {
            foreach (byte b in buffer)
            {
                if (b != 0)
                    return false;
            }
            return true;
        }

        private static void CheckConnection(Connection connection)
        {
            connection.EmptyBuffersCount++;
            if (connection.EmptyBuffersCount > 20)
                connection.Disconnect();
        }
    }
}
