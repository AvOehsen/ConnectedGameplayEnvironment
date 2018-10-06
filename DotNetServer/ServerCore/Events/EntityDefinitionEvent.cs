using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Cge.Server.Events
{
    [EntityEventType("definition")]
    public class EntityDefinitionEvent : AbstractEntityEvent
    {
        public string DeviceId { get; set; }
        public JObject[] Modules { get; set; }

    }
}
