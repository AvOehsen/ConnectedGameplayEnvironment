using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cge.Server.EntityModules;
using Cge.Server.Net;

namespace Cge.Server.Entities
{
    public class Entity
    {
        private readonly NetClient _net;
        private readonly int _entityId;
        private readonly AbstractEntityModule[] _modules;

        public int Id => _entityId;
        public IEnumerable<AbstractEntityModule> Modules => _modules;

        internal Entity(NetClient net, int id, AbstractEntityModule[] modules)
        {
            _net = net;
            _entityId = id;
            _modules = modules;
        }

    }
}
