using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cge.Server.Events
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class EntityEventTypeAttribute : Attribute
    {
        public string Type { get; }

        public EntityEventTypeAttribute(string type)
        {
            Type = type;
        }

    }
}
