using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.ObjectExtending.Modularity
{
    public static class ModuleExtensionConfigurationHelper
    {
        public static void ApplyEntityConfigurationToEntity(
            string moduleName,
            string entityName,
            Type entityType)
        {
            foreach (var propertyConfig in GetPropertyConfigurations(moduleName, entityName))
            {
                if (propertyConfig.Entity.IsAvailable &&
                    entityType != null)
                {
                    ApplyPropertyConfigurationToTypes(propertyConfig, new[] { entityType });
                }
            }
        }

        public static void ApplyEntityConfigurationToApi(
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

        public static void ApplyEntityConfigurationToUi(
            string moduleName,
            string entityName,
            Type[] createFormTypes = null,
            Type[] editFormTypes = null)
        {
            foreach (var propertyConfig in GetPropertyConfigurations(moduleName, entityName))
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

        public static void ApplyEntityConfigurations(
            string moduleName,
            string entityName,
            Type entityType = null,
            Type[] createFormTypes = null,
            Type[] editFormTypes = null,
            Type[] getApiTypes = null,
            Type[] createApiTypes = null,
            Type[] updateApiTypes = null)
        {
            if (entityType != null)
            {
                ApplyEntityConfigurationToEntity(
                    moduleName,
                    entityName,
                    entityType
                );
            }

            ApplyEntityConfigurationToApi(
                moduleName,
                entityName,
                getApiTypes: getApiTypes,
                createApiTypes: createApiTypes,
                updateApiTypes: updateApiTypes
            );

            ApplyEntityConfigurationToUi(
                moduleName,
                entityName,
                createFormTypes: createFormTypes,
                editFormTypes: editFormTypes
            );
        }

        [NotNull]
        public static IEnumerable<ExtensionPropertyConfiguration> GetPropertyConfigurations(
            string moduleName,
            string entityName)
        {
            var moduleConfig = ObjectExtensionManager.Instance.Modules().GetOrDefault(moduleName);
            if (moduleConfig == null)
            {
                return Array.Empty<ExtensionPropertyConfiguration>();
            }

            var objectConfig = moduleConfig.Entities.GetOrDefault(entityName);
            if (objectConfig == null)
            {
                return Array.Empty<ExtensionPropertyConfiguration>();
            }

            return objectConfig.GetProperties();
        }

        public static void ApplyPropertyConfigurationToTypes(
            ExtensionPropertyConfiguration propertyConfig,
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
