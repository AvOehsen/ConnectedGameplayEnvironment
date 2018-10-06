using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cge.Server.EntityModules
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class EntityModuleTypeAttribute : Attribute
    {
        public string Type { get; }

        public EntityModuleTypeAttribute(string type)
        {
            Type = type;
        }
    }
}
