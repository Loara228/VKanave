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
            Program.Log(LogType.Networking, $"Received {message}. ({connection.Index})");
            if (message != null)
            {
                if (message is NMAction)
                {
                    (message as NMAction)?.Action(connection);
                    Program.Log(LogType.NetworkingLow, $"{message}.Action() ({connection.Index})");
                }
            }
            else
            {
                Program.Log(LogType.Networking, $"Empty message {message}. ({connection.Index})", true);
            }
        }

        internal static void SendData(Connection connection, NetMessage message)
        {
            Program.Log(LogType.Networking, $"Sent {message}. ({connection.Index})");
            BinaryWriter writer = new BinaryWriter(connection.Stream, Encoding.UTF8);
            string dataStr = SerializeData(message);
            writer.Write(dataStr);
        }

        internal static NetMessage DeserializeData(string dataStr)
        {
            List<byte[]>? data = JsonConvert.DeserializeObject<List<byte[]>>(dataStr, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });

            string messageType = Encoding.UTF8.GetString(data[0]);

            Program.Log(LogType.Serialization, $"Deserializing {messageType}");
            Type t = Type.GetType(messageType);
            if (t != null)
            {
                NetMessage message = (NetMessage)Activator.CreateInstance(t);
                Program.Log(LogType.Serialization, $"Created {message}");
                message.Data = data;
                message.Deserialize();
                return message;
            }
            return null;
        }

        internal static string SerializeData(NetMessage message)
        {
            Program.Log(LogType.Serialization, $"Serializing buffer {message}");
            message.Serialize();
            Program.Log(LogType.Serialization, $"Serializing json {message}");
            return JsonConvert.SerializeObject(message.Data, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
        }
    }
}
