using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Reflection;

namespace Volo.Abp.AspNetCore.Mvc.ModelBinding
{
    public class AbpExtraPropertyModelBinder : IModelBinder
    {
        public Type ExtensibleObjectType { get; }

        public AbpExtraPropertyModelBinder(Type extensibleObjectType)
        {
            ExtensibleObjectType = extensibleObjectType;
        }

        public virtual Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            var model = ConvertStringToPropertyType(
                bindingContext,
                valueProviderResult.FirstValue
            );

            bindingContext.Result = ModelBindingResult.Success(model);

            return Task.CompletedTask;
        }

        protected virtual object ConvertStringToPropertyType(ModelBindingContext bindingContext, string value)
        {
            if (bindingContext.ModelMetadata.ConvertEmptyStringToNull && string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var extensionInfo = ObjectExtensionManager.Instance.GetOrNull(ExtensibleObjectType);
            if (extensionInfo == null)
            {
                return value;
            }

            var propertyName = ExtraPropertyBindingHelper.ExtractExtraPropertyName(bindingContext.ModelName);
            if (propertyName == null)
            {
                return value;
            }

            var propertyInfo = extensionInfo.GetPropertyOrNull(propertyName);
            if (propertyInfo == null)
            {
                return value;
            }

            return TypeHelper.ConvertFromString(
                propertyInfo.Type,
                value
            );
        }
    }
}