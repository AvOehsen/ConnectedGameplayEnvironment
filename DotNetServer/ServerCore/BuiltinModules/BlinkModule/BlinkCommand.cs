using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cge.Server.Commands;

namespace Cge.Server.BuiltinModules.BlinkModule
{
    public class BlinkCommand : AbstractModuleCommand
    {
        public enum BlinkState
        {
            On, Off
        }

        public BlinkState State { get; set; }

        public BlinkCommand() : base("blink_command")
        {
        }
    }
}
