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

        public bool IsLoadedAsPlugIn { get; }

        internal List<AbpModuleDescriptor> Dependencies { get; }

        public AbpModuleDescriptor([NotNull] Type type, [NotNull] IAbpModule instance, bool isLoadedAsPlugIn)
        {
            Check.NotNull(type, nameof(type));
            Check.NotNull(instance, nameof(instance));

            if (!type.GetTypeInfo().IsAssignableFrom(instance.GetType()))
            {
                throw new ArgumentException($"Given module instance ({instance.GetType().AssemblyQualifiedName}) is not an instance of given module type: {type.AssemblyQualifiedName}");
            }

            Type = type;
            Instance = instance;
            IsLoadedAsPlugIn = isLoadedAsPlugIn;

            Dependencies = new List<AbpModuleDescriptor>();
        }

        public override string ToString()
        {
            return $"[AbpModuleDescriptor {Type.FullName}]";
        }
    }
}
