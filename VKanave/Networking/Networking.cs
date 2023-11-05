using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.Networking.NetMessages;

namespace VKanave.Networking
{
    internal class Networking
    {
        internal static void ReceiveData()
        {
            BinaryReader reader = new BinaryReader(Connection.Current.Stream, Encoding.UTF8);
            NetMessage message = DeserializeData(reader.ReadString());
            if (message != null)
            {
                if (message is NMAction)
                    (message as NMAction).Action(Connection.Current);
            }
        }

        internal static void SendData(NetMessage data)
        {
            BinaryWriter writer = new BinaryWriter(Connection.Current.Stream, Encoding.UTF8);
            string dataStr = SerializeData(data);
            writer.Write(dataStr);
        }

        internal static NetMessage DeserializeData(string dataStr)
        {
            List<byte[]>? data = JsonConvert.DeserializeObject<List<byte[]>>(dataStr, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });

            string messageType = Encoding.UTF8.GetString(data[0]);

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
