using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cge.Server.Commands
{
    public abstract class AbstractModuleCommand : AbstractCommand
    {
        public int[] TargetModules { get; set; }

        protected AbstractModuleCommand(string type) : base(type)
        {
        }
    }
}
