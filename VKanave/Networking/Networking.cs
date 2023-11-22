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

            byte[] data = new byte[NetMessage.BUFFER_SIZE];
            bool emptyBuffer = true;
            while (true)
            {
                byte[] receivedData = new byte[NetMessage.BUFFER_SIZE];
                stream.Read(receivedData, 0, receivedData.Length);

                if (IsBufferEmpty(receivedData))
                {
                    data = data.Concat(receivedData).ToArray();
                    break;
                }
                else
                {
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
                return;
            }

            NetMessage msg = NetMessage.Create(data);
            msg.Deserialize();
            PrcMsg(Connection.Current, msg);
        }

        internal static void Send(NetMessage msg)
        {
            msg.Serialize();
            byte[] data = msg.Buffer;
            lock (_block)
                Connection.Current.Stream.Write(data, 0, data.Length);
        }

        internal static void PrcMsg(Connection from, NetMessage msg)
        {
            if (msg is NMAction)
                (msg as NMAction).Action(from);
        }

        private static bool IsBufferEmpty(byte[] buffer)
        {
            foreach(byte b in buffer)
            {
                if (b != 0)
                    return false;
            }
            return true;
        }

        private static object _block = new object();
    }
}
