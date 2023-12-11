using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.Networking
{
    internal static class LocalUser
    {
        public static void NewUser(string username, string token, long id, int reg, string displayName, string bio)
        {
            _username = username;
            _token = token;
            _id = id;
            Reg = reg;
            DisplayName = displayName;
            Bio = bio;
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

        public static string DisplayName
        {
            get; set;
        }

        public static string Bio
        {
            get; set;
        }

        public static int Reg
        {
            get; set;
        }

        private static long _id = 0;
        private static string _username;
        private static string _token;
    }
}
