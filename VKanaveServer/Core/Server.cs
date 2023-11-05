using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace VKanaveServer.Core
{
    internal class Server
    {
        private Server(IPAddress adress, uint port)
        {
            Connections = new List<Connection>();
            _listner = new TcpListener(IPAddress.Any, (int)port);
            _address = adress;
            _port = port;
        }

        internal static void Initialize(IPAddress adress, uint port)
        {
            Current = new Server(adress, port);
        }

        internal static void InitializeLocal()
        {
            Current = new Server(IPAddress.Parse(Program.IPAddress), 228);
        }

        internal void Start()
        {
            while(true)
            {
                _listner.Start();
                AcceptConnection();
            }
        }

        private void AcceptConnection()
        {
            Connection connection = Connection.CreateConnection(_listner.AcceptTcpClient());
            Connections.Add(connection);
        }

        internal static Server? Current
        {
            get; set;
        } = null;

        internal IPAddress Address
        {
            get => _address;
        }

        internal uint Port
        {
            get => _port;
        }

        internal bool Local
        {
            get; set;
        } = false;

        internal List<Connection> Connections
        {
            get; set;
        }

        private TcpListener _listner;
        private IPAddress _address;
        private uint _port;
    }
}
