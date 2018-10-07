using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cge.Server.EntityModules;

namespace Cge.Server.BuiltinModules.BlinkModule
{
    [EntityModuleType("blink")]
    public class BlinkModule : AbstractEntityModule
    {
        public override IEnumerable<Type> SupportedCommands
        {
            get { yield return typeof(BlinkCommand); }
        }
    }
}
