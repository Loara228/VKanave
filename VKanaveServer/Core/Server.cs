using Newtonsoft.Json.Linq;
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
        private Server(IPAddress address, uint port)
        {
            Connections = new List<Connection>();
            _listner = new TcpListener(IPAddress.Any, (int)port);
            _address = address;
            _port = port;
        }

        internal static void Initialize(IPAddress address = null, uint? port = null)
        {
            if (Program.LOCAL)
            {
                InitializeLocal();
                Program.Log(LogType.Networking, "Initialized");
                return;
            }

            if (address == null)
                throw new ArgumentException(nameof(address));
            if (port == null)
                throw new ArgumentException(nameof(port));

            Current = new Server(address, (uint)port);
            Program.Log(LogType.Networking, "Initialized");
        }

        private static void InitializeLocal()
        {
            Current = new Server(IPAddress.Parse(Program.IP), 228);
        }

        internal void Start()
        {
            Program.Log(LogType.Networking, $"Server starts");
            while (true)
            {
                _listner.Start();
                AcceptConnection();
            }
        }

        private void AcceptConnection()
        {
            Connection connection = Connection.CreateConnection(_listner.AcceptTcpClient());
            lock (_block)
            {
                Connections.Add(connection);
            }
        }

        public void RemoveConnection(Connection connection)
        {
            lock (_block)
            {
                Connections.Remove(connection);
            }
        }

        public Connection? GetConnectionFromUserId(long userId)
        {
            lock (_block)
            {
                foreach (Connection c in Connections)
                {
                    if (c.User.ID == userId)
                    {
                        return c;
                    }
                }
                return null;
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
