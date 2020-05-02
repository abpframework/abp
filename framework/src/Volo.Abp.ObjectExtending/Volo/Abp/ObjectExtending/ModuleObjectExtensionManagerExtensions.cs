using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.ObjectExtending
{
    public static class ModuleObjectExtensionManagerExtensions
    {
        private const string ObjectExtensionManagerConfigurationKey = "_Modules";

        public static ModuleObjectExtensionConfigurationDictionary Modules(
            [NotNull]this ObjectExtensionManager objectExtensionManager)
        {
            Check.NotNull(objectExtensionManager, nameof(objectExtensionManager));

            return objectExtensionManager.Configuration.GetOrAdd(
                ObjectExtensionManagerConfigurationKey,
                () => new ModuleObjectExtensionConfigurationDictionary()
            ) as ModuleObjectExtensionConfigurationDictionary;
        }
    }
}
