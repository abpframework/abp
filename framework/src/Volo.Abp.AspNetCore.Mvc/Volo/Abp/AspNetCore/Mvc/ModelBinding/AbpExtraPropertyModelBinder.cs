using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Volo.Abp.ObjectExtending;

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

            var propertyName = ExtractExtraPropertyName(bindingContext.ModelName);
            if (propertyName == null)
            {
                return value;
            }

            var propertyInfo = extensionInfo.GetPropertyOrNull(propertyName);
            if (propertyInfo == null)
            {
                return value;
            }

            return Convert.ChangeType(value, propertyInfo.Type);
        }

        /* modelName is a string like "UserInfo.ExtraProperties[SocialSecurityNumber]"
         * This method returns "SocialSecurityNumber" for this example. */
        protected virtual string ExtractExtraPropertyName(string modelName)
        {
            //TODO: Use regex(?) and add unit test (by extracting to a helper class)
            var index = modelName.IndexOf(".ExtraProperties[", StringComparison.Ordinal);
            if (index < 0)
            {
                return null;
            }

            return modelName.Substring(index + 17, modelName.Length - index - 18);
        }
    }
}