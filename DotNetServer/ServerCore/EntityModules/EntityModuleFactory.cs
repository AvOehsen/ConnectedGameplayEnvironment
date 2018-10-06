using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cge.Server.EntityModules
{
    internal static class EntityModuleFactory
    {
        private static Dictionary<string, Type> _moduleTypes = null;

        private static void EnsureTypes()
        {
            if (_moduleTypes == null)
            {
                _moduleTypes = new Dictionary<string, Type>();
                foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(AbstractEntityModule))))
                foreach (var attribute in type.GetCustomAttributes<EntityModuleTypeAttribute>())
                    _moduleTypes[attribute.Type] = type;
            }
        }

        internal static AbstractEntityModule BuildModule(JObject json)
        {
            if (!json.ContainsKey("type"))
                throw new JsonException("event message has to 'type' attribute!");

            string typeKey = json["type"].Value<string>();

            EnsureTypes();
            if (_moduleTypes.TryGetValue(typeKey, out var type))
                return (AbstractEntityModule) Activator.CreateInstance(type);
            else
                throw new JsonException($"Module type '{typeKey}' is unknown!");
        }
    }
}
