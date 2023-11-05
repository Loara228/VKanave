using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VKanave.DB;
using VKanave.Networking.NetMessages;
using VKanaveServer.Core;

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

        public override void Action(Connection from)
        {
            Exception exc = Database.RunCommand($"SELECT `user_id` FROM `users` WHERE `username` = '{username}'", out SQLTable table);
            if (exc != null)
                return;
            if (table.rows.Count == 0)
            {
                long unixTime = Database.GetUnixTime();
                string token = GenerateToken($"{username}{password}{unixTime}");
                exc = Database.RunCommand($"INSERT INTO `users` (`username`, `password_hash`, `reg`, `token`) VALUES ('{username}', '{password}', '{unixTime}', '{token}');", out _);
                if (exc != null)
                {
                    from.Send(new NMReg { code = 0 });
                    return;
                }
                from.Send(new NMReg { code = 1 });
            }
            else
            {
                from.Send(new NMReg { code = 2 });
            }
        }
        private byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        private string GenerateToken(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
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


//INSERT INTO `users` (`user_id`, `username`, `password_hash`, `reg`) VALUES ('1', 'usertest', 'password', '1699174800');