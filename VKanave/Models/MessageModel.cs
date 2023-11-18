﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.Extensions;
using VKanave.Networking.NetObjects;

namespace VKanave.Models
{
    public record class MessageModel(long ID, string Content, int UnixTime, ChatMessageFlags Flags)
    {
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
                if (Content.Length > 30)
                    return Content.Substring(0, 25) + "...";
                return Content;
            }
        }

        public Thickness Margin
        {
            get => Local ? new Thickness(40, 5, 10, 5) : new Thickness(10, 5, 40, 5);
        }

        public Color BackgroundColor
        {
            get
            {
                if (Local)
                    return Color.FromArgb("#879EEC");
                return Color.FromArgb("#272938");
            }
        }

        public Color DateTimeColor
        {
            get => Local ? Color.FromArgb("#C5DFFC") : Colors.Gray;
        }

        public bool Unread
        {
            get => Flags.HasFlag(ChatMessageFlags.UNREAD);
        }

        public string UnreadText
        {
            get
            {
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
    }
}