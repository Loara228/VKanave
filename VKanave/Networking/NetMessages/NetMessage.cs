using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.Networking.NetMessages
{
    [Serializable]
    public abstract class NetMessage
    {
        public void Serialize()
        {
            Type t = GetType();
            Write(t.FullName);
            foreach(var fieldInfo in t.GetFields())
            {
                if (fieldInfo.FieldType == typeof(int))
                {
                    Write((int)fieldInfo.GetValue(this));
                }
                else if (fieldInfo.FieldType == typeof(string))
                {
                    Write((string)fieldInfo.GetValue(this));
                }
            }
        }

        private void Write(string value)
        {
            byte[] data = Encoding.UTF8.GetBytes(value);
            Data.Add(data);
        }

        private void Write(int value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Data.Add(data);
        }

        public void Deserialize()
        {
            Type t = GetType();
            var fields = t.GetFields();
            for (int i = 1; i < Data.Count; i++)
            {
                Type fieldType = fields[i - 1].FieldType;
                if (fieldType == typeof(int))
                    fields[i - 1].SetValue(this, (int)BitConverter.ToInt32(Data[i], 0));
                else if (fieldType == typeof(string))
                    fields[i - 1].SetValue(this, (string)Encoding.UTF8.GetString(Data[i], 0, Data[i].Length));
            }
        }

        public List<byte[]> Data
        {
            get; set;
        } = new List<byte[]>();
    }
}
