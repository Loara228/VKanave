using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.DB;
using VKanaveServer.Core;


namespace VKanave.Networking.NetMessages
{
    public class NMLoadProfile : NMAction
    {
        public NMLoadProfile()
        {

        }

        public override void Action(Connection from)
        {
            _ = Database.RunCommand($"select `display_name`,`bio`,`reg` from `users` where `user_id` = {userId}", out var table);
            displayName = table.rows[0].values[0].ToString();
            bio = table.rows[0].values[1].ToString();
            reg = int.Parse(table.rows[0].values[2].ToString());
            from.Send(new NMLoadProfile() { userId = this.userId, displayName = this.displayName, bio = this.bio, reg = this.reg });
        }

        public long userId;
        public string displayName = "";
        public string bio = "";
        public int reg;

    }
}
