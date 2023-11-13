using VKanave.DB;
using VKanave.Networking.NetMessages;
using VKanave.Networking.NetObjects;
using VKanaveServer.Core;

namespace VKanaveServer
{
    internal class Program
    {
        //INSERT INTO `messages` (`id`, `msg_from`, `msg_to`, `date`, `content`, `flags`) VALUES (NULL, '3', '6', '1699488140', 'hello!', '1');
        static void Main(string[] args)
        {
            try
            {
                if (!Database.Initialize(Database.SQLHostname, out Exception exc, Database.SQLUsername, Database.SQLPassword))
                {
                    Log(LogType.SQL, exc.Message, true);
                }
                Server.InitializeLocal();
                Log(LogType.Console, "Initialized");
                Server.Current?.Start();
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc.ToString());
                Console.ReadLine();
            }
        }

        internal static void Log(LogType logType, string message, bool error = false)
        {
            Console.ForegroundColor = error ? ConsoleColor.Red : GetColor(logType);
            string t = "\t";
            if (logType == LogType.SQL)
                t += "\t";
            Console.WriteLine($"[{logType}]{t}{message}");
        }

        private static ConsoleColor GetColor(LogType log)
        {
            if (log == LogType.Console)
                return ConsoleColor.Gray;
            else if (log == LogType.Information)
                return ConsoleColor.Cyan;
            else if (log == LogType.SrlzHight)
                return ConsoleColor.Green;
            else if (log == LogType.SrlzLow)
                return ConsoleColor.DarkGreen;
            else if (log == LogType.Networking)
                return ConsoleColor.Yellow;
            else if (log == LogType.SQL)
                return ConsoleColor.Magenta;
            else if (log == LogType.Connection)
                return ConsoleColor.White;
            return ConsoleColor.White;
        }

        internal static string LocalIPAddress
        {
            get => WINDOWS ? "127.0.0.1" : "10.0.2.2";
        }

        public const bool LOCAL = true;
        public const bool WINDOWS = false;
    }
}