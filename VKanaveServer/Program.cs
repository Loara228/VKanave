using VKanave.DB;
using VKanave.Networking.NetMessages;
using VKanaveServer.Core;

namespace VKanaveServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (!Database.Initialize(Database.SQLHostname, out Exception exc, Database.SQLUsername, Database.SQLPassword))
                {
                    Log(LogType.SQL, exc.Message, true);
                }
                Server.InitializeLocal();
                Log(LogType.System, "Initialized");
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
            Console.ForegroundColor = error ? ConsoleColor.Red : GetLogColor(logType);
            string t = "\t";
            if (logType == LogType.SQL)
                t += "\t";
            Console.WriteLine($"[{logType}]{t}{message}");
        }

        private static ConsoleColor GetLogColor(LogType log)
        {
            if (log == LogType.System)
                return ConsoleColor.Gray;
            else if (log == LogType.Information)
                return ConsoleColor.DarkCyan;
            else if (log == LogType.Serialization)
                return ConsoleColor.Green;
            else if (log == LogType.Networking)
                return ConsoleColor.Yellow;
            else if (log == LogType.NetworkingLow)
                return ConsoleColor.DarkYellow;
            else if (log == LogType.SQL)
                return ConsoleColor.Magenta;
            else if (log == LogType.Connection)
                return ConsoleColor.White;
            return ConsoleColor.Black;
        }

        internal static string LocalIPAddress
        {
            get
            {
                if (WINDOWS)
                    return "127.0.0.1";
                else
                    return "10.0.2.2";
            }
        }

        public const bool LOCAL = true;
        public const bool WINDOWS = false;
    }
}