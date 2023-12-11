using System.Net;
using VKanave.DB;
using VKanave.Networking.NetMessages;
using VKanave.Networking.NetObjects;
using VKanaveServer.Core;

namespace VKanaveServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (OperatingSystem.IsWindows())
                WINDOWS = true;

            Log(LogType.Console, DateTime.UtcNow.AddHours(5).ToString());
            try
            {
                if (!Database.Initialize(out Exception exc))
                {
                    Log(LogType.SQL, exc.Message, true);
                }
                Server.Initialize(IPAddress.Any, 42069);
                Server.Current?.Start();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
                Console.ReadLine();
            }
        }

        internal static void Log(LogType logType, string message, bool error = false)
        {
            if (_disabledLogs.Contains(logType))
                return;
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
            else if (log == LogType.NetMessage)
                return ConsoleColor.Blue;
            else if (log == LogType.FileSystem)
                return ConsoleColor.DarkYellow;
            return ConsoleColor.White;
        }

        internal static string IP
        {
            get => LOCAL ? LocalIP : _ipAddress;
        }

        private static string LocalIP
        {
            get => WINDOWS ? "127.0.0.1" : "10.0.2.2";
        }

#if DEBUG
        public const bool LOCAL = true;
#else
        public const bool LOCAL = false;
#endif
        public static bool WINDOWS = false;

        private static string _ipAddress = string.Empty;

        private static readonly List<LogType> _disabledLogs = new List<LogType>()
        {
            LogType.SrlzLow,
            LogType.Networking,
            //LogType.SQL,
            //LogType.SrlzHight
        };
    }
}