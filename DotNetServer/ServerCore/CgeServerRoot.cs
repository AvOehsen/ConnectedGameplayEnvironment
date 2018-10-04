using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cge.Server.Entities;
using Cge.Server.Net;

namespace Cge.Server
{
    public class CgeServerRoot
    {

        private readonly NetServer _server;
        private readonly EntityManager _entityManager;

        public EntityManager EntityManager => _entityManager;

        public CgeServerRoot()
        {
            _server = new NetServer();
            _entityManager = new EntityManager();

            _server.ClientConnected += _entityManager.OnNewClientConnected;
        }

        
        public void StartServer(int port)
        {
            _server.Start(port);
        }

    }
}
