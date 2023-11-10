using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VKanaveServer;

namespace VKanave.Networking.NetMessages
{
    public abstract class NetMessage
    {
        public static NetMessage Create(byte[] newBuffer)
        {
            int position = 0;
            int size = 0;
            byte[] buffer = new byte[4];
            buffer[0] = newBuffer[position];
            buffer[1] = newBuffer[position + 1];
            buffer[2] = newBuffer[position + 2];
            buffer[3] = newBuffer[position + 3];
            size = BitConverter.ToInt32(buffer);
            position += 4;

            byte[] nmType = new byte[size];
            for (int i = 0; i < size; i++)
            {
                nmType[i] = newBuffer[position + i];
            }
            position += size;
            string nmTypeStr = Encoding.UTF8.GetString(nmType, 0, size);
            Type type = Type.GetType(nmTypeStr);
            NetMessage message = (NetMessage)Activator.CreateInstance(type);
            message._buffer = newBuffer;
            message._position = position;
            return message;
        }

        public void Serialize()
        {
            _position = 0;
            Type t = GetType();
            Write(t.FullName);
            foreach (var fieldInfo in t.GetFields())
            {
                if (fieldInfo.FieldType == typeof(int))
                {
                    var value = (int)fieldInfo.GetValue(this);
                    Program.Log(LogType.SerializationLow, $"Serialized {fieldInfo.Name}. Value: {value}");
                    Write(value);
                }
                else if (fieldInfo.FieldType == typeof(string))
                {
                    var value = (string)fieldInfo.GetValue(this);
                    Program.Log(LogType.SerializationLow, $"Serialized {fieldInfo.Name}. Value: {value}");
                    Write(value);
                }
            }
        }

        public void NMType()
        {
            ReadString();
        }

        public void Deserialize()
        {
            Type t = GetType();
            var fields = t.GetFields();
            Program.Log(LogType.Serialization, $"type: {t}. fields: {fields.Length}.");

            foreach (var field in fields)
            {
                if (field.FieldType == typeof(string))
                {
                    var value = (string)ReadString();
                    Program.Log(LogType.SerializationLow, $"Deserialized {field.Name}. Value: {value}");
                    field.SetValue(this, value);
                }
                else if (field.FieldType == typeof(int))
                {
                    var value = (int)ReadInt();
                    Program.Log(LogType.SerializationLow, $"Deserialized {field.Name}. Value: {value}");
                    field.SetValue(this, value);
                }
            }
        }

        protected virtual void OnSerialize()
        {
        }

        protected virtual void OnDeserialize()
        {

        }

        protected void Write(string value)
        {
            byte[] data = Encoding.UTF8.GetBytes(value);
            Write((short)data.Length);
            Write(data);
        }

        protected void Write(int value)
        {
            byte size = sizeof(int);
            if (_position + size > _buffer.Length)
                Resize(size);
            byte[] bytes = BitConverter.GetBytes(value);
            for (int i = 0; i < bytes.Length; i++)
            {
                _buffer[_position + i] = bytes[i];
            }
            _position += bytes.Length;
        }

        protected void Write(byte[] bytes)
        {
            int size = bytes.Length;
            if (_buffer.Length < _buffer.Length + bytes.Length)
            {
                Resize(size);
            }
            for (int i = 0; i < bytes.Length; i++)
            {
                _buffer[_position + i] = bytes[i];
            }
            _position += bytes.Length;
        }

        protected string ReadString()
        {
            int lenght = ReadInt();
            byte[] data = ReadBytes(lenght);
            return Encoding.UTF8.GetString(data, 0, lenght);
        }

        protected byte[] ReadBytes(int size)
        {
            byte[] data = new byte[size];
            for (int i = 0; i < size; i++)
            {
                data[i] = _buffer[_position + i];
            }
            _position += size;
            return data;
        }

        protected int ReadInt()
        {
            byte[] buffer = new byte[4];
            buffer[0] = _buffer[_position];
            buffer[1] = _buffer[_position + 1];
            buffer[2] = _buffer[_position + 2];
            buffer[3] = _buffer[_position + 3];
            int value = BitConverter.ToUInt16(buffer);
            _position += 4;
            return value;
        }

        protected void Resize(int bytes)
        {
            int reqBytes = _buffer.Count() * 2;
            while (reqBytes < bytes)
                reqBytes *= 2;

            byte[] newBytes = new byte[reqBytes];
            _buffer.CopyTo(newBytes, 0);
            _buffer = newBytes;
        }

        public byte[] Buffer
        {
            get => _buffer;
        }

        private byte[] _buffer = new byte[1024];
        private int _position;
    }
}
