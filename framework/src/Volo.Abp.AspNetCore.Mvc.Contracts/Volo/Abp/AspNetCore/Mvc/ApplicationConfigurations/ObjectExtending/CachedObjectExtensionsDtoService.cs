using System.Collections.Generic;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Reflection;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending
{
    public class CachedObjectExtensionsDtoService : ICachedObjectExtensionsDtoService, ISingletonDependency
    {
        private volatile ObjectExtensionsDto _cachedValue;
        private readonly object _syncLock = new object();

        public virtual ObjectExtensionsDto Get()
        {
            if (_cachedValue == null)
            {
                lock (_syncLock)
                {
                    if (_cachedValue == null)
                    {
                        _cachedValue = GenerateCacheValue();
                    }
                }
            }

            return _cachedValue;
        }

        protected virtual ObjectExtensionsDto GenerateCacheValue()
        {
            //TODO: Obviously needs refactoring!

            var objectExtensionsDto = new ObjectExtensionsDto
            {
                Modules = new Dictionary<string, ModuleExtensionDto>()
            };

            foreach (var moduleConfig in ObjectExtensionManager.Instance.Modules)
            {
                var moduleExtensionDto = objectExtensionsDto.Modules[moduleConfig.Key] = new ModuleExtensionDto
                {
                    Objects = new Dictionary<string, ModuleObjectExtensionDto>()
                };

                foreach (var objectConfig in moduleConfig.Value)
                {
                    var moduleObjectExtensionDto = moduleExtensionDto.Objects[objectConfig.Key] =
                        new ModuleObjectExtensionDto
                        {
                            ExtraProperties = new Dictionary<string, ModuleObjectExtraPropertyExtensionDto>()
                        };

                    foreach (var propertyConfig in objectConfig.Value.GetProperties())
                    {
                        var propertyExtensionDto = moduleObjectExtensionDto.ExtraProperties[propertyConfig.Name] =
                            new ModuleObjectExtraPropertyExtensionDto
                            {
                                Type = TypeHelper.GetFullNameHandlingNullableAndGenerics(propertyConfig.Type),
                                TypeSimple = TypeHelper.GetSimplifiedName(propertyConfig.Type),
                                Attributes = new List<ModuleObjectExtraPropertyAttributeDto>(), 
                                DisplayName = CreateDisplayNameDto(propertyConfig),
                                Ui = new ModuleObjectExtraPropertyUiExtensionDto
                                {
                                    CreateForm = new ModuleObjectExtraPropertyUiFormExtensionDto
                                    {
                                        IsVisible = propertyConfig.UI.CreateForm.IsVisible
                                    },
                                    EditForm = new ModuleObjectExtraPropertyUiFormExtensionDto
                                    {
                                        IsVisible = propertyConfig.UI.EditForm.IsVisible
                                    },
                                    Table = new ModuleObjectExtraPropertyUiTableExtensionDto
                                    {
                                        IsVisible = propertyConfig.UI.Table.IsVisible
                                    }
                                }
                            };

                        foreach (var attribute in propertyConfig.Attributes)
                        {
                            propertyExtensionDto.Attributes.Add(
                                ModuleObjectExtraPropertyAttributeDto.Create(attribute)
                            );
                        }
                    }
                }
            }

            return objectExtensionsDto;
        }

        private static LocalizableStringDto CreateDisplayNameDto(
            ModuleEntityObjectPropertyExtensionConfiguration propertyConfig)
        {
            if (propertyConfig.DisplayName == null)
            {
                return null;
            }

            if (propertyConfig.DisplayName is LocalizableString localizableStringInstance)
            {
                return new LocalizableStringDto
                {
                    Name = localizableStringInstance.Name,
                    Resource = LocalizationResourceNameAttribute.GetName(localizableStringInstance.ResourceType)
                };
            }

            if (propertyConfig.DisplayName is FixedLocalizableString fixedLocalizableString)
            {
                return new LocalizableStringDto
                {
                    Name = fixedLocalizableString.Value,
                    Resource = "_"
                };
            }

            return null;
        }
    }
}
