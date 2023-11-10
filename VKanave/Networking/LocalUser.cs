using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.Networking
{
    internal static class LocalUser
    {
        public static void NewUser(string username, string token)
        {
            _username = username;
            _token = token;
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

        private static string _username;
        private static string _token;
    }
}
