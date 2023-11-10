using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.Models
{
    public record class ChatModel(UserModel User, MessageModel LastMessage);
}
