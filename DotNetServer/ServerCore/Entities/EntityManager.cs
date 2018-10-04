using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cge.Server.Net;
using Newtonsoft.Json.Linq;

namespace Cge.Server.Entities
{
    public class EntityManager
    {
        private readonly List<Entity> _entities = new List<Entity>();
        private readonly List<NetClient> _unboundClients = new List<NetClient>();

        private int _nextEntityId = 1;

        public EntityManager()
        {
        }

        internal void OnNewClientConnected(NetClient client)
        {
            client.MessageSent += OnUnboundClientMessageSent;
            _unboundClients.Add(client);
        }

        private void OnUnboundClientMessageSent(NetClient client, JObject message)
        {
            //see if the unique device ID is already bound to an entity
        }
    }
}
