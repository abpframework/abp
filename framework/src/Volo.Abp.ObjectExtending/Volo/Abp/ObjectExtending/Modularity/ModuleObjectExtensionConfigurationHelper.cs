using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.ObjectExtending.Modularity
{
    public static class ModuleObjectExtensionConfigurationHelper
    {
        public static void ApplyModuleObjectExtensionConfigurationToEntity(
            string moduleName,
            string objectName,
            Type entityType)
        {
            foreach (var propertyConfig in GetPropertyConfigurations(moduleName, objectName))
            {
                if (propertyConfig.Entity.IsAvailable &&
                    entityType != null)
                {
                    ApplyPropertyConfigurationToTypes(propertyConfig, new[] { entityType });
                }
            }
        }

        public static void ApplyModuleObjectExtensionConfigurationToApi(
            string moduleName,
            string objectName,
            Type[] getApiTypes = null,
            Type[] createApiTypes = null,
            Type[] updateApiTypes = null)
        {
            foreach (var propertyConfig in GetPropertyConfigurations(moduleName, objectName))
            {
                if (!propertyConfig.IsAvailableToClients)
                {
                    continue;
                }

                if (propertyConfig.Api.OnGet.IsAvailable &&
                    getApiTypes != null)
                {
                    ApplyPropertyConfigurationToTypes(propertyConfig, getApiTypes);
                }

                if (propertyConfig.Api.OnCreate.IsAvailable &&
                    createApiTypes != null)
                {
                    ApplyPropertyConfigurationToTypes(propertyConfig, createApiTypes);
                }

                if (propertyConfig.Api.OnUpdate.IsAvailable &&
                    updateApiTypes != null)
                {
                    ApplyPropertyConfigurationToTypes(propertyConfig, updateApiTypes);
                }
            }
        }

        public static void ApplyModuleObjectExtensionConfigurationToUI(
            string moduleName,
            string objectName,
            Type[] createFormTypes = null,
            Type[] editFormTypes = null)
        {
            foreach (var propertyConfig in GetPropertyConfigurations(moduleName, objectName))
            {
                if (!propertyConfig.IsAvailableToClients)
                {
                    continue;
                }

                if (propertyConfig.UI.OnCreateForm.IsVisible &&
                    createFormTypes != null)
                {
                    ApplyPropertyConfigurationToTypes(propertyConfig, createFormTypes);
                }

                if (propertyConfig.UI.OnEditForm.IsVisible &&
                    editFormTypes != null)
                {
                    ApplyPropertyConfigurationToTypes(propertyConfig, editFormTypes);
                }
            }
        }

        public static void ApplyModuleObjectExtensionConfigurations(
            string moduleName,
            string objectName,
            Type entityType = null,
            Type[] createFormTypes = null,
            Type[] editFormTypes = null,
            Type[] getApiTypes = null,
            Type[] createApiTypes = null,
            Type[] updateApiTypes = null)
        {
            if (entityType != null)
            {
                ApplyModuleObjectExtensionConfigurationToEntity(
                    moduleName,
                    objectName,
                    entityType
                );
            }

            ApplyModuleObjectExtensionConfigurationToApi(
                moduleName,
                objectName,
                getApiTypes: getApiTypes,
                createApiTypes: createApiTypes,
                updateApiTypes: updateApiTypes
            );

            ApplyModuleObjectExtensionConfigurationToUI(
                moduleName,
                objectName,
                createFormTypes: createFormTypes,
                editFormTypes: editFormTypes
            );
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
