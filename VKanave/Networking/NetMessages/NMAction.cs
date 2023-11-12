using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.Networking.NetMessages
{
    public abstract class NMAction : NetMessage
    {
        public virtual void Action(Connection from)
        {
        }

        public bool TokenRequired
        {
            get => _tokenRequired;
            set
            {
                if (value)
                    _token = LocalUser.Token;
                _tokenRequired = value;
            }
        }

        public string _token = string.Empty;
        private bool _tokenRequired;
    }
}
