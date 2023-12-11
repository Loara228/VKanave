using Microsoft.Maui.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.Extensions;
using VKanave.Networking.NetObjects;

namespace VKanave.Models
{
    public record class MessageModel(long ID, string Content, int UnixTime) : INotifyPropertyChanged
    {
        public MessageModel(long ID, string Content, int UnixTime, ChatMessageFlags Flags) : this(ID, Content, UnixTime)
        {
            this.Flags = Flags;
        }

        public void Delete()
        {
            if (Flags.HasFlag(ChatMessageFlags.DELETED) || Flags.HasFlag(ChatMessageFlags.SYSTEM))
                return;
            Flags = Flags | ChatMessageFlags.SYSTEM | ChatMessageFlags.DELETED;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Unread"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UnreadText"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Margin"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TextColor"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BackgroundColor"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewMessage"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DateTimeColor"));
        }

        public ChatMessageFlags Flags
        {
            get; set;
        }

        public string Text
        {
            get
            {
                if (Flags.HasFlag(ChatMessageFlags.DELETED))
                    return "MESSAGE DELETED";
                return Content;
            }
        }

        public DateTime DateTime
        {
            get
            {
                return UnixTime.ToDateTime();
            }
        }

        public string Preview
        {
            get
            {
                string str = Flags.HasFlag(ChatMessageFlags.OUTBOX) ? "You: " : "";
                if (Content.Length > 30)
                    return str + Content.Substring(0, 25) + "...";
                return str + Content;
            }
        }

        public Thickness Margin
        {
            get
            {
                if (SystemMessage)
                    return new Thickness(5);
                    return Local ? new Thickness(40, 5, 10, 5) : new Thickness(10, 5, 40, 5);
            }
        }

        public Color BackgroundColor
        {
            get
            {
                if (SystemMessage)
                    return Colors.Transparent;
                if (Local)
                    return Color.FromArgb("#879EEC");
                return Color.FromArgb("#272938");
            }
        }

        public Color DateTimeColor
        {
            get
            {
                if (SystemMessage)
                    return Colors.Transparent;
                return Local ? Color.FromArgb("#C5DFFC") : Colors.Gray;
            }
        }

        public Color TextColor
        {
            get
            {
                if (SystemMessage)
                    return Colors.Gray;
                return Colors.White;
            }
        }

        public bool Unread
        {
            get
            {
                if (SystemMessage)
                    return false;
                if (_unread == null)
                    return Flags.HasFlag(ChatMessageFlags.UNREAD);
                return (bool)_unread;
            }
            set
            {
                _unread = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Unread"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UnreadText"));
                
            }
        }

        public bool NewMessage
        {
            get
            {
                if (SystemMessage)
                    return false;
                return Flags.HasFlag(ChatMessageFlags.UNREAD) && !Flags.HasFlag(ChatMessageFlags.OUTBOX);
            }
        }

        public string UnreadText
        {
            get
            {
                if (SystemMessage)
                    return "";
                if (_unread != null && _unread == false)
                    return "";
                if (Flags.HasFlag(ChatMessageFlags.UNREAD))
                {
                    if (Flags.HasFlag(ChatMessageFlags.OUTBOX))
                        return "Unread";
                    return "New";
                }
                return "";
            }
        }

        public bool Local
        {
            get => Flags.HasFlag(ChatMessageFlags.OUTBOX);
        }

        public bool SystemMessage
        {
            get => Flags.HasFlag(ChatMessageFlags.SYSTEM);
        }

        private bool? _unread = null;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
