using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cge.Server.EntityModules;

namespace Cge.Server.BuiltinModules.IrReceiver
{
    [EntityModuleType("ir_receiver")]
    public class IrReceiverModule : AbstractEntityModule
    {
        public override IEnumerable<Type> SupportedCommands
        {
            get
            {
                yield return typeof(EnableIrCommand);
                yield return typeof(DisableIrCommand);
            }
        }
    }
}
