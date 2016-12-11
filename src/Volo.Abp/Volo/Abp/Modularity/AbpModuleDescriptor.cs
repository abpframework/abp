using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace Volo.Abp.Modularity
{
    public class AbpModuleDescriptor
    {
        public Type Type { get; }

        public IAbpModule Instance { get; }

        internal List<AbpModuleDescriptor> Dependencies { get; }

        public AbpModuleDescriptor([NotNull] Type type, [NotNull] IAbpModule instance)
        {
            Check.NotNull(type, nameof(type));
            Check.NotNull(instance, nameof(instance));

            if (!type.GetTypeInfo().IsAssignableFrom(instance.GetType()))
            {
                throw new ArgumentException($"Given module instance ({instance.GetType().AssemblyQualifiedName}) is not an instance of given module type: {type.AssemblyQualifiedName}");
            }

            Type = type;
            Instance = instance;

            Dependencies = new List<AbpModuleDescriptor>();
        }
    }
}
