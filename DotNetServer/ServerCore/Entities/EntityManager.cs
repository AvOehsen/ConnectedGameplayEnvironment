using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cge.Server.Commands;
using Cge.Server.EntityModules;
using Cge.Server.Events;
using Cge.Server.Net;
using Newtonsoft.Json.Linq;

namespace Cge.Server.Entities
{
    public class EntityManager
    {
        private readonly List<Entity> _entities = new List<Entity>();
        private readonly List<NetClient> _unboundClients = new List<NetClient>();

        private int _nextEntityId = 1;

        public event Action<Entity> EntityAdded;

        public EntityManager()
        {
        }

        internal void OnNewClientConnected(NetClient client)
        {
            client.EntityEventSent += OnUnboundClientEventSent;
            lock (_unboundClients)
                _unboundClients.Add(client);
        }

        private void OnUnboundClientEventSent(NetClient client, AbstractEntityEvent evt)
        {
            if (evt is EntityDefinitionEvent definitionEvent)
            {
                //TODO: manage to build something to handle disconnect / reconnect of devices (might be useful for energy saving)
                //TODO: see if the unique device ID is already bound to an entity and reuse it
                
                AbstractEntityModule[] modules = new AbstractEntityModule[definitionEvent.Modules.Length];
                for (int i = 0; i < modules.Length; i++)
                    modules[i] = EntityModuleFactory.BuildModule(definitionEvent.Modules[i]);

                var newEntity = new Entity(client, _nextEntityId++, definitionEvent.DeviceId, modules);
                
                client.EntityEventSent -= OnUnboundClientEventSent;
                lock (_unboundClients)
                    _unboundClients.Remove(client);

                newEntity.SendCommand(new EntityConfigCommand(newEntity.Id));   //TODO: see if modules need to add something to the config ?

                lock (_entities)
                    _entities.Add(newEntity);

                if(EntityAdded != null)
                    lock (EntityAdded)
                        EntityAdded(newEntity);
            }
            else
            {
                //this entity sent an event before if was declared as an entity. This is not good.
                //TODO: handle this exception somehow gracefully
            }
            
        }
    }
}
