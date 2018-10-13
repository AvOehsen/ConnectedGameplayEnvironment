using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cge.Server.Commands;

namespace Cge.Server.BuiltinModules.IrReceiver
{
    public class EnableIrCommand : AbstractModuleCommand
    {
        public EnableIrCommand() : base("ir_enable")
        {
        }
    }
}
