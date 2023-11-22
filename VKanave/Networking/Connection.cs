using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.Networking
{
    public class Connection
    {
        private Connection(string hostname, uint port)
        {
            Client = new TcpClient();
            _hostname = hostname;
            _port = port;
        }

        internal static void Initialize()
        {
            if (MauiProgram.LOCAL)
            {
                InitializeLocal();
                return;
            }
            Current = new Connection(MauiProgram.IP_ADDRESS, 42069);
        }

        private static void InitializeLocal()
        {
#if WINDOWS
            Current = new Connection("127.0.0.1", 228);
#endif
#if ANDROID
            Current = new Connection("10.0.2.2", 228);
#endif
        }

        internal bool Connect()
        {
            bool result = false;
            try
            {
                Current.Client.Connect(_hostname, (int)_port);
                result = true;
                new Thread(() =>
                {
                    try
                    {
                        while (true)
                        {
                            Networking.ReceiveData();
                        }
                    }
                    catch
                    {
                        Application.Current.Quit();
                    }
                }).Start();
            }
            catch
            {
                result = false;
            }
            return result;
        }

        internal static Connection Current
        {
            get; set;
        }

        internal NetworkStream Stream
        {
            get => Client.GetStream();
        }

        private TcpClient Client
        {
            get; set;
        }

        private readonly string _hostname;
        private readonly uint _port;
    }
}
