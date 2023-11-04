using VKanaveServer.Core;

namespace VKanaveServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server.InitializeLocal();
            Console.WriteLine("init");
            Server.Current?.Start();
        }
    }
}