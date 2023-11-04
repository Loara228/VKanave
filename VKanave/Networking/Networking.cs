using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.Networking
{
    internal class Networking
    {
        internal static void ReceiveData()
        {
            BinaryReader reader = new BinaryReader(Connection.Current.Stream, Encoding.UTF8);
            DeserializeData(reader.ReadString());
        }

        internal static void SendData(object data)
        {
            BinaryWriter writer = new BinaryWriter(Connection.Current.Stream, Encoding.UTF8);
            string dataStr = SerializeData(data);
            writer.Write(dataStr);
        }

        internal static void DeserializeData(string data)
        {

        }

        internal static string SerializeData(object data)
        {
            return data.ToString();
        }
    }
}
