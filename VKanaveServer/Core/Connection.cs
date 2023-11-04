using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace VKanaveServer.Core
{
    internal class Connection
    {
        private Connection(TcpClient client)
        {
            Client = client;
        }

        private void Start()
        {
            new Thread(() =>
            {
                while(true)
                {
                    Console.WriteLine("Reading...");
                    Networking.ReceiveData(this);
                }
            }).Start();
        }

        internal static Connection CreateConnection(TcpClient client)
        {
            Connection connection = new Connection(client);
            connection.Start();
            return connection;
        }

        internal TcpClient Client
        {
            get; set;
        }

        internal NetworkStream Stream
        {
            get => Client.GetStream();
        }
    }
}
