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

        internal static void Initialize(string hostname, uint port)
        {
            Current = new Connection(hostname, port);
        }

        internal static void InitializeLocal()
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
                    while (true)
                    {
                        Networking.ReceiveData();
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
