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
            Program.Log(LogType.Networking, "Initialized");
        }

        internal static void InitializeLocal()
        {
            Current = new Server(IPAddress.Parse(Program.LocalIPAddress), 228);
            Program.Log(LogType.Networking, "Initialized");
        }

        internal void Start()
        {
            Program.Log(LogType.Networking, "Running");
            while (true)
            {
                _listner.Start();
                AcceptConnection();
            }
        }

        private void AcceptConnection()
        {
            Connection connection = Connection.CreateConnection(_listner.AcceptTcpClient());
            lock(_block)
            {
                Connections.Add(connection);
            }
        }

        public void RemoveConnection(Connection connection)
        {
            lock(_block)
            {
                Connections.Remove(connection);
            }
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

        private static object _block = new object();

        private TcpListener _listner;
        private IPAddress _address;
        private uint _port;
    }
}
