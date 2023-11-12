using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.Networking.NetObjects
{
    public class ChatUser
    {
        public ChatUser(long userId, string username)
        {
            this.User = userId;
            this.Username = username;
        }

        public long User
        {
            get; private set;
        }

        public string Username
        {
            get; private set;
        } = "";
    }
}
