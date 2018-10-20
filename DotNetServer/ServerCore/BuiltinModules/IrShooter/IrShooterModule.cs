using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cge.Server.Commands;
using Cge.Server.EntityModules;

namespace Cge.Server.BuiltinModules.IrShooter
{
    [EntityModuleType("ir_shooter")]
    public class IrShooterModule : AbstractEntityModule
    {
        public override IEnumerable<Type> SupportedCommands
        {
            get { yield return typeof(ShooterConfigCommand); }
        }
    }
}
