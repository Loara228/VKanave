using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.Extensions;

namespace VKanave.Models
{
    public record class MessageModel(long ID, string Content, long UnixTime, int Flags = 0)
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
    }
}
