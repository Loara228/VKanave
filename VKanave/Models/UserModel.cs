using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.Models
{
    public record UserModel(long ID, int unixTime)
    {
        public UserModel(string Username, long ID, int unixTime) : this(ID, unixTime)
        {
            this._username = Username;
        }
        public UserModel(string Username, long ID, int unixTime, string DisplayName) : this(ID, unixTime)
        {
            this._username = Username;
            this.displayName = DisplayName;
        }

        public string Username
        {
            get
            {
                if (string.IsNullOrEmpty(displayName))
                {
                    return _username;
                }
                return displayName;
            }
        }

        public string displayName, _username;
    }
}
