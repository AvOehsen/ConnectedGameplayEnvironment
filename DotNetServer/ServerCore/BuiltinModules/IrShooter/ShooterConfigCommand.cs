using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cge.Server.Commands;

namespace Cge.Server.BuiltinModules.IrShooter
{
    public class ShooterConfigCommand : AbstractModuleCommand
    {
        public ShooterConfigCommand() : base("ir_shooter_config")
        {
        }

        public int Warmup { get; set; }
        public int SalveDelay { get; set; }
        public int SalveCount { get; set; }
        public int Cooldown { get; set; }
    }
}
