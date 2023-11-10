using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using VKanave.Networking.NetMessages;

namespace VKanaveServer.Core
{
    public class Connection
    {
        private Connection(TcpClient client)
        {
            Client = client;
            Index = _connectionIndexCount;
            _connectionIndexCount++;
            Program.Log(LogType.Connection, $"Connected ({Index})");
        }

        public void Send(NetMessage message)
        {
            Networking.Send(this, message);
        }

        private void Start()
        {
            new Thread(() =>
            {
                while(true)
                {
                    try
                    {
                        Networking.ReceiveData(this);
                    }
                    catch(Exception exc)
                    {
                        Program.Log(LogType.NetworkingLow, exc.ToString(), true);
                        Disconnect();
                        break;
                    }
                }
            }).Start();
        }

        private void Disconnect()
        {
            try
            {
                Client.Close();
            }
            catch
            {
                Server.Current?.RemoveConnection(this);
            }
            Program.Log(LogType.Connection, $"Disconnect ({Index})");
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

        internal int Index
        {
            get; set;
        }

        private static int _connectionIndexCount;
    }
}
