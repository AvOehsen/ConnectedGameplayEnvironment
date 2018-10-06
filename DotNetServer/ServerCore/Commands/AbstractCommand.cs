using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cge.Server.Commands
{
    public abstract class AbstractCommand
    {
        public string Type { get; }

        protected AbstractCommand(string type)
        {
            Type = type;
        }
    }
}
