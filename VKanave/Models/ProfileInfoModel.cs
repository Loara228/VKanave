using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.Extensions;
using VKanave.Networking;
using VKanave.Networking.NetMessages;

namespace VKanave.Models
{
    public class ProfileInfoModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Local
        /// </summary>
        public ProfileInfoModel()
        {
            Username = LocalUser.Username;
            Reg = LocalUser.Reg.ToDateTime().ToString();
            DisplayName = LocalUser.DisplayName;
            Bio = LocalUser.Bio;
            _lastActive = -1; // local value
            userId = -1; // local value
        }
        public ProfileInfoModel(string username, int lastActive, long userId)
        {
            this.Username = username;
            this._lastActive = lastActive;
            this.userId = userId;
        }

        private string GetLastSeenTitle(int unixTime)
        {
            DateTime dt = unixTime.ToDateTime();
            if (DateTime.Now < dt.AddMinutes(2))
                return "Online";
            return $"Last seen at " + dt.ToString();
        }

        public string Username
        {
            get => "@" + _username;
            set
            {
                _username = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Username"));
            }
        }

        public string LastActive
        {
            get
            {
                if (_lastActive == -1)
                    return "Online";
                else if (_lastActive == 0)
                    return "Last seen recently";
                return GetLastSeenTitle(_lastActive);
            }
            set
            {
                _username = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LastActive"));
            }
        }

        public string Bio
        {
            get
            {
                if (string.IsNullOrEmpty(_bio))
                {
                    return "Empty";
                }
                return _bio;
            }
            set
            {
                _bio = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Bio"));
            }
        }

        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(_displayName))
                {
                    return Username;
                }
                return _displayName;
            }
            set
            {
                _displayName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayName"));
            }
        }

        public string Reg
        {
            get => _reg;
            set
            {
                _reg = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Reg"));
            }
        }

        public long userId;

        private string _username, _displayName, _bio, _reg;
        private int _lastActive = 0;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
