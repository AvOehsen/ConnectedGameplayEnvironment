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

        public NetServer(int port)
        {
            _listenerSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _listenerSocket.Bind(new IPEndPoint(GetLocalIpAddress(), port));

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

            //TODO: event new connected client

            _listenerSocket.BeginAccept(OnNewConnection, this);
        }
    }
}
