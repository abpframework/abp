using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.AspNetCore.Mvc.ModelBinding.Metadata
{
    public class AbpExtraPropertyValidationMetadataProvider : IValidationMetadataProvider
    {
        public static AsyncLocal<ExtraPropertyModelInfo> CurrentModeInfo { get; } = new AsyncLocal<ExtraPropertyModelInfo>();

        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            if (CurrentModeInfo.Value == null)
            {
                return;
            }

            var currentModel = CurrentModeInfo.Value;

            CurrentModeInfo.Value = null;

            var propertyInfo = ObjectExtensionManager.Instance.GetPropertyOrNull(
                currentModel.ExtensibleObjectType,
                currentModel.PropertyName
            );

            if (propertyInfo == null)
            {
                return;
            }

            foreach (var validationAttribute in propertyInfo.ValidationAttributes)
            {
                context.ValidationMetadata.ValidatorMetadata.Add(validationAttribute);
            }
        }

        public class ExtraPropertyModelInfo
        {
            public Type ExtensibleObjectType { get; }
            public string PropertyName { get; }

            public ExtraPropertyModelInfo(
                Type extensibleObjectType,
                string propertyName)
            {
                ExtensibleObjectType = extensibleObjectType;
                PropertyName = propertyName;
            }
        }
    }
}
