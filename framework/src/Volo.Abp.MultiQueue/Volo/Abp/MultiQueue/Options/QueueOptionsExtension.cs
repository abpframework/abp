using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Volo.Abp.MultiQueue.Options;
public static class QueueOptionsExtension
{
    internal readonly static Dictionary<string, Type> QueueOptionsTypeMap = new Dictionary<string, Type>();

    public static Type GetOptionType(string key)
    {
        QueueOptionsTypeMap.TryGetValue(key, out var type);
        return type;
    }

    public static Type GetOrCreateOptionType<TQueueOptions>(string key) where TQueueOptions : class, IQueueOptions
    {
        var type = GetOptionType(key);
        if (type == null)
        {
            AssemblyName assemblyName = new AssemblyName("Volo.Abp.MultiQueue.Options");
            AssemblyBuilder dyAssembly = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);

            ModuleBuilder dyModule = dyAssembly.DefineDynamicModule("DynamicOptions");

            TypeBuilder dyClass = dyModule.DefineType($"{key}QueueOptions", TypeAttributes.Public | TypeAttributes.Serializable | TypeAttributes.Class | TypeAttributes.AutoClass, typeof(TQueueOptions), new Type[] { typeof(IQueueOptions) });

            type = dyClass.CreateTypeInfo();
            QueueOptionsTypeMap.Add(key, type);
        };
        return type;
    }

    public static JToken GetOptions(IConfigurationSection config)
    {
        JObject obj = new JObject();
        var children = config.GetChildren();
        foreach (var child in children)
        {
            obj.Add(child.Key, GetOptions(child));
        }

        if (!obj.HasValues && config is IConfigurationSection section)
            return new JValue(section.Value);

        return obj;
    }

    public static List<QueueOptionsWarp> GetQueueOptions(IConfigurationSection config)
    {
        var jToken = GetOptions(config);
        var options = jToken.ToObject<Dictionary<string, QueueOptionsWarp>>();

        if (options == null)
            options = new Dictionary<string, QueueOptionsWarp>();

        if (options != null && options.Count > 0)
        {
            foreach (var item in options)
            {
                item.Value.Key = item.Key;
            }
        }
        return options.Select(p => p.Value).ToList();
    }
}
