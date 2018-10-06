using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cge.Server.Events
{
    internal static class EntityEventFactory
    {
        private static Dictionary<string, Type> _eventTypes = null;

        private static void EnsureTypes()
        {
            if (_eventTypes == null)
            {
                _eventTypes = new Dictionary<string, Type>();
                foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(AbstractEntityEvent))))
                foreach (var attribute in type.GetCustomAttributes<EntityEventTypeAttribute>())
                    _eventTypes[attribute.Type] = type;
            }
        }

        internal static AbstractEntityEvent BuildEvent(JObject json)
        {
            if (!json.ContainsKey("type"))
                throw new JsonException("event message has to 'type' attribute!");

            string typeKey = json["type"].Value<string>();

            EnsureTypes();
            if (_eventTypes.TryGetValue(typeKey, out Type type))
                return (AbstractEntityEvent) json.ToObject(type);
            else
                throw new JsonException($"event type '{typeKey}' is unknown!");
        }
    }
}
