using System;
using System.Collections.Generic;
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
    public class Entity
    {
        private readonly NetClient _net;
        private readonly AbstractEntityModule[] _modules;
        private readonly List<AbstractCommand> _sentCommands = new List<AbstractCommand>();

        public int Id { get; }
        public string DeviceId { get; }

        public IEnumerable<AbstractEntityModule> Modules => _modules;

        public IEnumerable<AbstractCommand> SentCommands
        {
            get
            {
                lock (_sentCommands)
                    return _sentCommands.ToArray();
            }
        }

        public IEnumerable<AbstractEntityEvent> ReceivedEvents => _net.Events;

        internal Entity(NetClient net, int id, string deviceId, AbstractEntityModule[] modules)
        {
            Id = id;
            DeviceId = deviceId;

            _modules = modules;
            for (int i = 0; i < _modules.Length; i++)
                _modules[i].SetOwner(this, i);

            _net = net;
        }

        public void SendCommand(AbstractCommand command)
        {
            _net.Send(command);
            lock (_sentCommands)
                _sentCommands.Add(command);
        }

        public override string ToString()
        {
            return $"{Id} - {DeviceId} ({_net.Ip})";
        }
    }
}
