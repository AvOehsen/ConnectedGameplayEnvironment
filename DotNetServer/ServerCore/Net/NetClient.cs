﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cge.Server.Net
{
    internal class NetClient
    {
        private readonly NetServer _server;
        private readonly Socket _socket;

        private readonly List<JObject> _messages = new List<JObject>();


        private readonly byte[] _readBuffer = new byte[1024];
        private string _stringBuffer = "";

        
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
                string[] messages = _stringBuffer.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                for (int i = 0; i < messages.Length - 1; i++)
                {
                    try
                    {
                        var message = JObject.Parse(messages[i]);
                        lock (_messages)
                            _messages.Add(message);

                        //TODO: message received event

                    }
                    catch (JsonReaderException ex)
                    {
                        //TODO: handle failed json parse
                    }
                }

                _stringBuffer = messages.Last();
            }
        }
    }
}