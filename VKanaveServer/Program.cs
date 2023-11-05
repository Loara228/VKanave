using VKanave.Networking.NetMessages;
using VKanaveServer.Core;

namespace VKanaveServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //https://github.com/TheFlyingFoool/DuckGameRebuilt/blob/master/DuckGame/src/DuckGame/Network/NetMessage.cs
            //return;
            try
            {
                Server.InitializeLocal();
                Log(LogType.System, "init");
                Server.Current?.Start();
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc.ToString());
                Console.ReadLine();
            }
        }

        internal static void Log(LogType logType, string message)
        {
            Console.WriteLine($"[{logType}]\t{message}");
        }

        internal static string IPAddress
        {
            get
            {
                if (WINDOWS)
                    return "127.0.0.1";
                else
                    return "10.0.2.2";
            }
        }

        private const bool WINDOWS = true;
    }
}