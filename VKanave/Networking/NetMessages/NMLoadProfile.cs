using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.Extensions;
using VKanave.Views;

namespace VKanave.Networking.NetMessages
{
    public class NMLoadProfile : NMAction
    {
        public NMLoadProfile()
        {

        }

        public override void Action(Connection from)
        {
            var profilePage = ProfilePage.Current;
            if (profilePage != null && profilePage._profileInfo.userId == this.userId)
            {
                profilePage._profileInfo.DisplayName = displayName;
                profilePage._profileInfo.Bio = bio;
                profilePage._profileInfo.Reg = reg.ToDateTime().ToString();
            }
        }

        public long userId;
        public string displayName = "";
        public string bio = "";
        public int reg;

    }
}
