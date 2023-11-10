using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.Views;

namespace VKanave.Networking.NetMessages
{
    [Serializable]
    public class NMAuth : NMAction
    {
        internal NMAuth(string username, string password)
        {
            this.username = username;
            this.password = password;
            token = string.Empty;
        }

        public NMAuth()
        {
            username = string.Empty;
            password = string.Empty;
            token = string.Empty;
        }

        public override void Action(Connection connection)
        {
            if (LoginPage.Current != null)
            {
                LoginPage.Current.SignIn(username, token);
            }
        }

        public string username, password, token;

    }
}
