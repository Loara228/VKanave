using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;  
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using VKanave.Networking.NetMessages;

namespace VKanaveServer.Core
{
    internal static class Networking
    {
        internal static void ReceiveData(Connection connection)
        {
            BinaryReader reader = new BinaryReader(connection.Stream, Encoding.UTF8);
            NetMessage? message = DeserializeData(reader.ReadString());
            if (message != null)
            {
                Program.Log(LogType.Networking, $"Deserialized {message}. ({connection.Index})");
                if (message is NMAction)
                {
                    (message as NMAction)?.Action(connection);
                    Program.Log(LogType.Networking, $"{message}.Action()");
                }
            }
            else
            {
                Program.Log(LogType.Networking, $"Empty message {message}. ({connection.Index})");
            }
        }

        internal static void SendData(Connection connection, NetMessage message)
        {
            BinaryWriter writer = new BinaryWriter(connection.Stream, Encoding.UTF8);
            string dataStr = SerializeData(message);
            writer.Write(dataStr);
        }

        internal static NetMessage DeserializeData(string dataStr)
        {
            List<byte[]>? data = JsonConvert.DeserializeObject<List<byte[]>>(dataStr, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });

            string messageType = Encoding.UTF8.GetString(data[0]);

            Program.Log(LogType.NetworkingLow, $"Received: {messageType}");
            Type t = Type.GetType(messageType);
            if (t != null)
            {
                NetMessage message = (NetMessage)Activator.CreateInstance(t);
                message.Data = data;
                message.Deserialize();
                return message;
            }
            return null;
        }

        internal static string SerializeData(NetMessage message)
        {
            message.Serialize();
            return JsonConvert.SerializeObject(message.Data, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
        }
    }
}
