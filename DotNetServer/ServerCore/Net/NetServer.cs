using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Cge.Server.Net
{
    internal class NetServer
    {
        private readonly Socket _listenerSocket;
        private readonly List<NetClient> _conntectClients = new List<NetClient>();

        internal event Action<NetClient> ClientConnected;

        public NetServer()
        {
            _listenerSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        internal void Start(int port)
        {
            _listenerSocket.Bind(new IPEndPoint(GetLocalIpAddress(), port));
            _listenerSocket.Listen(10);
            _listenerSocket.BeginAccept(OnNewConnection, this);
        }

        internal void RemoveClient(NetClient client)
        {
            lock (_conntectClients)
                _conntectClients.Remove(client);
        }

        private IPAddress GetLocalIpAddress()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            return ipHostInfo.AddressList.First(a => a.AddressFamily == AddressFamily.InterNetwork);
        }

        private void OnNewConnection(IAsyncResult ar)
        {
            var clientSocket = _listenerSocket.EndAccept(ar);
            var netClient = new NetClient(this, clientSocket);

            lock(_conntectClients)
                _conntectClients.Add(netClient);

            if(ClientConnected != null)
                lock (ClientConnected)
                    ClientConnected(netClient);

            _listenerSocket.BeginAccept(OnNewConnection, this);
        }
    }
}
