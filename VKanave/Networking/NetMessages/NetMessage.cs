using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.Networking.NetObjects;

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
                    Write(value);
                }
                else if (fieldInfo.FieldType == typeof(string))
                {
                    var value = (string)fieldInfo.GetValue(this);
                    Write(value);
                }
                else if (fieldInfo.FieldType == typeof(long))
                {
                    var value = (long)fieldInfo.GetValue(this);
                    Write(value);
                }
            }
            OnSerialize();
            Write(new byte[BUFFER_SIZE]);
        }

        public void Deserialize()
        {
            Type t = GetType();
            var fields = t.GetFields();

            foreach (var field in fields)
            {
                if (field.FieldType == typeof(string))
                {
                    var value = (string)ReadString();
                    field.SetValue(this, value);
                }
                else if (field.FieldType == typeof(int))
                {
                    var value = (int)ReadInt();
                    field.SetValue(this, value);
                }
                else if (field.FieldType == typeof(long))
                {
                    var value = (long)ReadLong();
                    field.SetValue(this, value);
                }
            }
            OnDeserialize();
        }

        protected virtual void OnSerialize()
        {
        }

        protected virtual void OnDeserialize()
        {

        }

        #region Write

        protected void Write(string value)
        {
            byte[] data = Encoding.UTF8.GetBytes(value);
            Write((short)data.Length);
            Write(data);
        }

        protected void Write(int value)
        {
            byte size = sizeof(int);
            Resize(size);
            byte[] bytes = BitConverter.GetBytes(value);
            for (int i = 0; i < bytes.Length; i++)
            {
                _buffer[_position + i] = bytes[i];
            }
            _position += bytes.Length;
        }

        protected void Write(long value)
        {
            byte size = sizeof(long);
            Resize(size);
            byte[] bytes = BitConverter.GetBytes(value);
            for (int i = 0; i < bytes.Length; i++)
            {
                _buffer[_position + i] = bytes[i];
            }
            _position += bytes.Length;
        }

        protected void Write(ChatMessage message)
        {
            Write(message.User);
            Write(message.ID);
            Write(message.Content);
            Write(message.Date);
            Write(message.Flags);
        }

        protected void Write(ChatUser user)
        {
            Write(user.ID);
            Write(user.Username);
            Write(user.LastActive);
        }

        protected void Write(byte[] bytes)
        {
            int size = bytes.Length;
            Resize(size);
            for (int i = 0; i < bytes.Length; i++)
            {
                _buffer[_position + i] = bytes[i];
            }
            _position += bytes.Length;
        }

        #endregion

        #region Write

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
            int value = BitConverter.ToInt32(buffer);
            _position += 4;
            return value;
        }

        protected long ReadLong()
        {
            byte[] buffer = new byte[8];
            buffer[0] = _buffer[_position];
            buffer[1] = _buffer[_position + 1];
            buffer[2] = _buffer[_position + 2];
            buffer[3] = _buffer[_position + 3];
            buffer[4] = _buffer[_position + 4];
            buffer[5] = _buffer[_position + 5];
            buffer[6] = _buffer[_position + 6];
            buffer[7] = _buffer[_position + 7];
            long value = BitConverter.ToInt64(buffer);
            _position += 8;
            return value;
        }

        protected ChatUser ReadUser()
        {
            long id = ReadLong();
            string username = ReadString();
            int lastActive = ReadInt();
            return new ChatUser(id, username, lastActive);
        }

        protected ChatMessage ReadChatMessage()
        {
            ChatUser user = ReadUser();
            long id = ReadLong();
            string content = ReadString();
            int date = ReadInt();
            int flags = ReadInt();
            return new ChatMessage(user, id, content, date, flags);
        }

        #endregion

        private void Resize(int needBytes)
        {
            while (_position + needBytes > _buffer.Length)
                Array.Resize(ref _buffer, _buffer.Length + BUFFER_SIZE);
        }

        public byte[] Buffer
        {
            get => _buffer;
        }

        public const int BUFFER_SIZE = 64;

        private byte[] _buffer = new byte[BUFFER_SIZE];
        private int _position;
    }
}
