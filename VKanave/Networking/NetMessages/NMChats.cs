using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.Networking.NetMessages
{
    public class NMChats : NMAction
    {
        public NMChats()
        {

        }

        public override void Action(Connection from)
        {

        }

        protected override void OnSerialize()
        {
            Write();
        }

        public int CountOfChats
        {
            get => _chatCount;
            set => _chatCount = value;
        }

        public string[] Users
        {
            get => _users;
            set => _users = value;
        }

        public int[] Dates
        {
            get => _dates;
            set => _dates = value;
        }

        public int[] Flags
        {
            get => _flags;
            set => _flags = value;
        }

        private string[] _users;
        private int[] _dates;
        private int[] _flags;

        private int _chatCount = 0;
    }
}
