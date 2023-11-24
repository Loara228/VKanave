using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.Networking
{
    internal static class LocalUser
    {
        public static void NewUser(string username, string token, long id)
        {
            _username = username;
            _token = token;
            _id = id;
            AppShell.UpdateLocalInfo(_username);
        }

        public static string Username
        {
            get => _username;
        }

        /// <summary>
        /// Токен, отвечающий за авторизацию. Без него вы не имеете доступа к любым действиям (по идеи)
        /// </summary>
        public static string Token
        {
            get => _token;
        }

        public static long Id
        {
            get => _id;
        }

        private static long _id = 0;
        private static string _username;
        private static string _token;
    }
}
