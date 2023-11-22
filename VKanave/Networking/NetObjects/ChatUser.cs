using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.Networking.NetObjects
{
    public class ChatUser
    {
        public ChatUser(long userId, string username, int lastActive)
        {
            this.ID = userId;
            this.Username = username;
            this.LastActive = lastActive;
        }

        public long ID
        {
            get; private set;
        }

        public string Username
        {
            get; private set;
        } = "";

        public int LastActive
        {
            get; private set;
        } = 0;
    }
}
