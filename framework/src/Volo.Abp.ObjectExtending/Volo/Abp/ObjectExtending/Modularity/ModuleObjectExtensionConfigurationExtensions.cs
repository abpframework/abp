using System;
using System.Collections.Generic;

namespace Volo.Abp.ObjectExtending.Modularity
{
    public static class ModuleObjectExtensionConfigurationExtensions
    {
        public static T ConfigureObject<T>(
            this T objectConfiguration,
            string objectName,
            Action<ModuleEntityObjectExtensionConfiguration> configureAction)
            where T : ModuleObjectExtensionConfiguration
        {
            var configuration = objectConfiguration.Entities.GetOrAdd(
                objectName,
                () => new ModuleEntityObjectExtensionConfiguration()
            );

            configureAction(configuration);

            return objectConfiguration;
        }
    }
}