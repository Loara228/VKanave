using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanaveServer.Core
{
    internal static class Networking
    {
        internal static void ReceiveData(Connection connection)
        {
            BinaryReader reader = new BinaryReader(connection.Stream, Encoding.UTF8);
            DeserializeData(reader.ReadString());
        }

        internal static void SendData(Connection connection, object data)
        {
            BinaryWriter writer = new BinaryWriter(connection.Stream, Encoding.UTF8);
            string dataStr = SerializeData(data);
            writer.Write(dataStr);
        }

        internal static void DeserializeData(string data)
        {
            Console.WriteLine($"Message received: {data}.");
        }

        internal static string SerializeData(object data)
        {
            return data.ToString();
        }
    }
}
