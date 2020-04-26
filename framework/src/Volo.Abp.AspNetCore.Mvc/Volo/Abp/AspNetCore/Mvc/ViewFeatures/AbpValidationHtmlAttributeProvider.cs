using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.ModelBinding;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation.Localization;

namespace Volo.Abp.AspNetCore.Mvc.ViewFeatures
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(ValidationHtmlAttributeProvider))]
    public class AbpValidationHtmlAttributeProvider 
        : DefaultValidationHtmlAttributeProvider, ISingletonDependency
    {
        private readonly IModelMetadataProvider _metadataProvider;
        private readonly IStringLocalizerFactory _stringLocalizerFactory;
        private readonly IStringLocalizer<AbpValidationResource> _validationStringLocalizer;
        private readonly IValidationAttributeAdapterProvider _validationAttributeAdapterProvider;

        public AbpValidationHtmlAttributeProvider(
            IOptions<MvcViewOptions> optionsAccessor, 
            IModelMetadataProvider metadataProvider, 
            ClientValidatorCache clientValidatorCache,
            IValidationAttributeAdapterProvider validationAttributeAdapterProvider, 
            IStringLocalizerFactory stringLocalizerFactory, 
            IStringLocalizer<AbpValidationResource> validationStringLocalizer) 
            : base(
                optionsAccessor, 
                metadataProvider, 
                clientValidatorCache)
        {
            _metadataProvider = metadataProvider;
            _validationAttributeAdapterProvider = validationAttributeAdapterProvider;
            _stringLocalizerFactory = stringLocalizerFactory;
            _validationStringLocalizer = validationStringLocalizer;
        }

        public override void AddValidationAttributes(
            ViewContext viewContext, 
            ModelExplorer modelExplorer, 
            IDictionary<string, string> attributes)
        {
            base.AddValidationAttributes(viewContext, modelExplorer, attributes);

            var nameAttribute = attributes.GetOrDefault("name");
            var propertyName = ExtraPropertyBindingHelper.ExtractExtraPropertyName(nameAttribute);
            if (propertyName == null)
            {
                return;
            }

            //NOTE: containerName can be null on controller actions..?

            var containerName = ExtraPropertyBindingHelper.ExtractContainerName(nameAttribute);
            if (containerName == null)
            {
                return;
            }

            var extensibleObjectType = modelExplorer.Container.ModelType
                .GetProperty(containerName, BindingFlags.Instance | BindingFlags.Public)
                .PropertyType;

            var propertyInfo = ObjectExtensionManager.Instance.GetPropertyOrNull(extensibleObjectType, propertyName);

            modelExplorer.Metadata.As<DefaultModelMetadata>().DisplayMetadata.DisplayName = () => propertyInfo.DisplayName.Localize(_stringLocalizerFactory);

            foreach (var validationAttribute in propertyInfo.ValidationAttributes)
            {
                var validationContext = new ClientModelValidationContext(
                    viewContext,
                    modelExplorer.Metadata,
                    _metadataProvider,
                    attributes);

                validationAttribute.ErrorMessage = ValidationAttributeErrorMessageStringProperty.GetValue(validationAttribute) as string;

                _validationAttributeAdapterProvider.GetAttributeAdapter(validationAttribute, _validationStringLocalizer)
                    .AddValidation(validationContext);
            }
        }

        private static readonly PropertyInfo ValidationAttributeErrorMessageStringProperty = typeof(ValidationAttribute)
            .GetProperty("ErrorMessageString", BindingFlags.Instance | BindingFlags.NonPublic);

    }
}
