using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Cge.Server.Commands;
using Cge.Server.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cge.Server.Net
{
    internal class NetClient
    {
        private readonly NetServer _server;
        private readonly Socket _socket;

        private readonly List<AbstractEntityEvent> _events = new List<AbstractEntityEvent>();


        private readonly byte[] _readBuffer = new byte[1024];
        private string _stringBuffer = "";

        internal event Action<NetClient, AbstractEntityEvent> EntityEventSent;

        internal IEnumerable<AbstractEntityEvent> Events
        {
            get
            {
                lock (_events)
                    return _events.ToArray();
            }
        }

        public string Ip
        {
            get
            {
                if (_socket.RemoteEndPoint is IPEndPoint ip)
                    return ip.Address.MapToIPv4().ToString();
                else
                    return "IP unknown";
            }
        }


        public NetClient(NetServer server, Socket socket)
        {
            _server = server;
            _socket = socket;

            _socket.BeginReceive(_readBuffer, 0, _readBuffer.Length, SocketFlags.None, OnReceived, this);
        }

        private void OnReceived(IAsyncResult ar)
        {
            int bytes = _socket.EndReceive(ar);

            ParseBuffer(bytes);

            _socket.BeginReceive(_readBuffer, 0, _readBuffer.Length, SocketFlags.None, OnReceived, this);
        }

        private void ParseBuffer(int bytes)
        {
            _stringBuffer += Encoding.ASCII.GetString(_readBuffer, 0, bytes);
            if (_stringBuffer.Contains(Environment.NewLine))
            {
                string[] messages = _stringBuffer.Split(new string[] {Environment.NewLine}, StringSplitOptions.None);
                for (int i = 0; i < messages.Length - 1; i++)
                {
                    try
                    {
                        if (messages[i].StartsWith("["))
                        {
                            var array = JArray.Parse(messages[i]);
                            foreach (var token in array)
                            {
                                var message = token.ToObject<JObject>();
                                ProcessObject(message);
                            }
                        }
                        else
                        {
                            var message = JObject.Parse(messages[i]);
                            ProcessObject(message);
                        }
                    }
                    catch (JsonException ex)
                    {
                        //TODO: better handle failed json parse
                        Console.WriteLine($"error reading json message from '{_socket.RemoteEndPoint}': {ex.Message}");
                    }
                }

                _stringBuffer = messages.Last();
            }
        }

        private void ProcessObject(JObject message)
        {
            var evt = EntityEventFactory.BuildEvent(message);

            lock (_events)
                _events.Add(evt);

            if (EntityEventSent != null)
                lock (EntityEventSent)
                    EntityEventSent(this, evt);
        }

        internal void Send(AbstractCommand command)
        {
            var jsonString = JsonConvert.SerializeObject(command) + Environment.NewLine;
            var bytes = Encoding.ASCII.GetBytes(jsonString);

            _socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, OnSent, this);
        }

        private void OnSent(IAsyncResult ar)
        {
            _socket.EndSend(ar);

            //TODO: error handling
        }
    }
}