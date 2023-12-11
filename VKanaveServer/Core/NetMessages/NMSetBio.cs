using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.DB;
using VKanaveServer.Core;


namespace VKanave.Networking.NetMessages
{
    public class NMSetBio : NMAction
    {
        public NMSetBio()
        {
            TokenRequired = true;
        }

        public override void Action(Connection from)
        {
            base.Action(from);

            Database.RunCommand($"update `users` set `bio` = '{bio}' where user_id = {userId}", out var _);
        }

        public long userId = 0;
        public string bio = string.Empty;

    }
}