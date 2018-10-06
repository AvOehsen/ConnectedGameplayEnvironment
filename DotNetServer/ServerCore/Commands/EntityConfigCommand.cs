using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cge.Server.Commands
{
    public class EntityConfigCommand : AbstractCommand
    {
        public int EntityId { get; }

        public EntityConfigCommand(int entityId) : base("config")
        {
            EntityId = entityId;
        }
    }
}
