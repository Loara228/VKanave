using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanave.Views;

namespace VKanave.Networking.NetMessages
{
    [Serializable]
    public class NMReg : NMAction
    {
        public NMReg()
        {
            username = string.Empty;
            password = string.Empty;
            code = 0;
        }

        public NMReg(string username, string password)
        {
            this.username = username;
            this.password = password;
            code = 0;
        }

        public override void Action(Connection from)
        {
            if (LoginPage.Current != null)
            {
                LoginPage.Current.SignUp(code);
            }
        }

        /// <summary>
        /// Код, с помощью которого определяется результат регистрации
        ///  <para>0   unknown error</para>
        ///  <para>1   Successfully</para>
        ///  <para>2   Already exists</para>
        /// </summary>
        public int code;
        public string username, password;
    }
}
