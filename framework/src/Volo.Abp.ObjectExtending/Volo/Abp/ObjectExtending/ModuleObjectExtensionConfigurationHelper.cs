using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.ObjectExtending
{
    public static class ModuleObjectExtensionConfigurationHelper
    {
        public static void ApplyModuleObjectExtensionConfigurations(
            string moduleName,
            string objectName,
            Type[] createFormTypes = null,
            Type[] editFormTypes = null
        )
        {
            foreach (var propertyConfig in GetPropertyConfigurations(moduleName, objectName))
            {
                if (propertyConfig.UI.CreateForm.IsVisible &&
                    createFormTypes != null)
                {
                    ApplyPropertyConfigurationToTypes(propertyConfig, createFormTypes);
                }

                if (propertyConfig.UI.EditForm.IsVisible &&
                    editFormTypes != null)
                {
                    ApplyPropertyConfigurationToTypes(propertyConfig, editFormTypes);
                }
            }
        }

        [NotNull]
        public static IEnumerable<ModuleEntityObjectPropertyExtensionConfiguration> GetPropertyConfigurations(
            string moduleName, 
            string objectName)
        {
            var moduleConfig = ObjectExtensionManager.Instance.Modules.GetOrDefault(moduleName);
            if (moduleConfig == null)
            {
                return Array.Empty<ModuleEntityObjectPropertyExtensionConfiguration>();
            }

            var objectConfig = moduleConfig.GetOrDefault(objectName);
            if (objectConfig == null)
            {
                return Array.Empty<ModuleEntityObjectPropertyExtensionConfiguration>();
            }

            return objectConfig.GetProperties();
        }

        public static void ApplyPropertyConfigurationToTypes(
            ModuleEntityObjectPropertyExtensionConfiguration propertyConfig,
            Type[] types)
        {
            ObjectExtensionManager.Instance
                .AddOrUpdateProperty(
                    types,
                    propertyConfig.Type,
                    propertyConfig.Name,
                    property =>
                    {
                        property.Attributes.AddRange(propertyConfig.Attributes);
                        property.DisplayName = propertyConfig.DisplayName;
                        property.Validators.AddRange(propertyConfig.Validators);
                    }
                );
        }
    }
}
