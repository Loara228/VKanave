using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.Networking.NetObjects;
using VKanave.Views.Popups;

namespace VKanave.Networking.NetMessages
{
    public class NMNewChat : NMAction
    {
        public NMNewChat()
        {
            TokenRequired = true;
        }

        public override void Action(Connection from)
        {
            base.Action(from);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                AddUserPopup.Current.DisplayResult((NewChatResult)result);
            });
        }

        public string username = string.Empty;

        /// <summary>
        /// enum NetChatResult
        /// </summary>
        public int result;

    }
}
