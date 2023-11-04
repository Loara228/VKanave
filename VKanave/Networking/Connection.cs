using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.Networking
{
    internal class Connection
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
            Current = new Connection("127.0.0.1", 228);
        }

        internal void Connect()
        {
            Current.Client.Connect(_hostname, (int)_port);
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

        private string _hostname;
        private uint _port;
    }
}
