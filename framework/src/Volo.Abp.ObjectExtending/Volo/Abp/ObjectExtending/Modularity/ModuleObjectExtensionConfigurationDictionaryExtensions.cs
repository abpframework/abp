using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.ObjectExtending.Modularity
{
    public static class ModuleObjectExtensionConfigurationDictionaryExtensions
    {
        public static ModuleObjectExtensionConfigurationDictionary ConfigureModule<T>(
            [NotNull] this ModuleObjectExtensionConfigurationDictionary configurationDictionary,
            [NotNull] string moduleName,
            [NotNull] Action<T> configureAction)
            where T : ModuleObjectExtensionConfiguration, new()
        {
            Check.NotNull(moduleName, nameof(moduleName));
            Check.NotNull(configureAction, nameof(configureAction));

            configureAction(
                (T)configurationDictionary.GetOrAdd(
                    moduleName,
                    () => new T()
                )
            );

            return configurationDictionary;
        }
    }
}