using System.Collections.Generic;
using System.Linq;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Reflection;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending
{
    public class CachedObjectExtensionsDtoService : ICachedObjectExtensionsDtoService, ISingletonDependency
    {
        protected IExtensionPropertyAttributeDtoFactory ExtensionPropertyAttributeDtoFactory { get; }
        protected volatile ObjectExtensionsDto CachedValue;
        protected readonly object SyncLock = new object();

        public CachedObjectExtensionsDtoService(IExtensionPropertyAttributeDtoFactory extensionPropertyAttributeDtoFactory)
        {
            ExtensionPropertyAttributeDtoFactory = extensionPropertyAttributeDtoFactory;
        }

        public virtual ObjectExtensionsDto Get()
        {
            if (CachedValue == null)
            {
                lock (SyncLock)
                {
                    if (CachedValue == null)
                    {
                        CachedValue = GenerateCacheValue();
                    }
                }
            }

            return CachedValue;
        }

        protected virtual ObjectExtensionsDto GenerateCacheValue()
        {
            var objectExtensionsDto = new ObjectExtensionsDto
            {
                Modules = new Dictionary<string, ModuleExtensionDto>()
            };

            foreach (var moduleConfig in ObjectExtensionManager.Instance.Modules())
            {
                objectExtensionsDto.Modules[moduleConfig.Key] = CreateModuleExtensionDto(moduleConfig.Value);
            }

            return objectExtensionsDto;
        }

        protected virtual ModuleExtensionDto CreateModuleExtensionDto(
            ModuleExtensionConfiguration moduleConfig)
        {
            var moduleExtensionDto = new ModuleExtensionDto
            {
                Entities = new Dictionary<string, EntityExtensionDto>()
            };

            foreach (var objectConfig in moduleConfig.Entities)
            {
                moduleExtensionDto.Entities[objectConfig.Key] = GetEntityExtensionDto(objectConfig.Value);
            }

            foreach (var customConfig in moduleConfig.Configuration.Where(c => !c.Key.StartsWith("_")))
            {
                moduleExtensionDto.Configuration[customConfig.Key] = customConfig.Value;
            }

            return moduleExtensionDto;
        }

        protected virtual EntityExtensionDto GetEntityExtensionDto(
            EntityExtensionConfiguration entityConfig)
        {
            var entityExtensionDto = new EntityExtensionDto
            {
                Properties = new Dictionary<string, ExtensionPropertyDto>(),
                Configuration = new Dictionary<string, object>()
            };

            foreach (var propertyConfig in entityConfig.GetProperties())
            {
                if (!propertyConfig.IsAvailableToClients)
                {
                    continue;
                }

                entityExtensionDto.Properties[propertyConfig.Name] = CreateExtensionPropertyDto(propertyConfig);
            }

            foreach (var customConfig in entityConfig.Configuration.Where(c => !c.Key.StartsWith("_")))
            {
                entityExtensionDto.Configuration[customConfig.Key] = customConfig.Value;
            }

            return entityExtensionDto;
        }

        protected virtual ExtensionPropertyDto CreateExtensionPropertyDto(
            ExtensionPropertyConfiguration propertyConfig)
        {
            var extensionPropertyDto = new ExtensionPropertyDto
            {
                Type = TypeHelper.GetFullNameHandlingNullableAndGenerics(propertyConfig.Type),
                TypeSimple = TypeHelper.GetSimplifiedName(propertyConfig.Type),
                Attributes = new List<ExtensionPropertyAttributeDto>(),
                DisplayName = CreateDisplayNameDto(propertyConfig),
                Configuration = new Dictionary<string, object>(),
                Api = new ExtensionPropertyApiDto
                {
                    OnGet = new ExtensionPropertyApiGetDto
                    {
                        IsAvailable = propertyConfig.Api.OnGet.IsAvailable
                    },
                    OnCreate = new ExtensionPropertyApiCreateDto
                    {
                        IsAvailable = propertyConfig.Api.OnCreate.IsAvailable
                    },
                    OnUpdate = new ExtensionPropertyApiUpdateDto
                    {
                        IsAvailable = propertyConfig.Api.OnUpdate.IsAvailable
                    }
                },
                Ui = new ExtensionPropertyUiDto
                {
                    OnCreateForm = new ExtensionPropertyUiFormDto
                    {
                        IsVisible = propertyConfig.UI.OnCreateForm.IsVisible
                    },
                    OnEditForm = new ExtensionPropertyUiFormDto
                    {
                        IsVisible = propertyConfig.UI.OnEditForm.IsVisible
                    },
                    OnTable = new ExtensionPropertyUiTableDto
                    {
                        IsVisible = propertyConfig.UI.OnTable.IsVisible
                    }
                }
            };

            foreach (var attribute in propertyConfig.Attributes)
            {
                extensionPropertyDto.Attributes.Add(
                    ExtensionPropertyAttributeDtoFactory.Create(attribute)
                );
            }

            foreach (var customConfig in propertyConfig.Configuration.Where(c => !c.Key.StartsWith("_")))
            {
                extensionPropertyDto.Configuration[customConfig.Key] = customConfig.Value;
            }

            return extensionPropertyDto;
        }

        protected virtual LocalizableStringDto CreateDisplayNameDto(ExtensionPropertyConfiguration propertyConfig)
        {
            if (propertyConfig.DisplayName == null)
            {
                return null;
            }

            if (propertyConfig.DisplayName is LocalizableString localizableStringInstance)
            {
                return new LocalizableStringDto(
                    localizableStringInstance.Name,
                    localizableStringInstance.ResourceType != null
                        ? LocalizationResourceNameAttribute.GetName(localizableStringInstance.ResourceType)
                        : null
                );
            }

            if (propertyConfig.DisplayName is FixedLocalizableString fixedLocalizableString)
            {
                // "_" means don't use the default resource, but directly use the name.
                return new LocalizableStringDto(fixedLocalizableString.Value, "_");
            }

            return null;
        }
    }
}
