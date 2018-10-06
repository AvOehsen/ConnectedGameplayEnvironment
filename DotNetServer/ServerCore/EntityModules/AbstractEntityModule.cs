using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cge.Server.Entities;

namespace Cge.Server.EntityModules
{
    public abstract class AbstractEntityModule
    {
        protected Entity Owner { get; private set; }
        protected int ModuleIndex { get; private set; }

        //TODO

        internal void SetOwner(Entity owner, int moduleIndex)
        {
            Owner = owner;
            ModuleIndex = moduleIndex;
        }
    }
}
