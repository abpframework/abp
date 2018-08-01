using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class AbpDataAnnotationAutoLocalizationMetadataDetailsProvider : IDisplayMetadataProvider
    {
        private readonly Lazy<IStringLocalizerFactory> _stringLocalizerFactory;
        private readonly Lazy<IOptions<MvcDataAnnotationsLocalizationOptions>> _localizationOptions;

        public AbpDataAnnotationAutoLocalizationMetadataDetailsProvider(IServiceCollection services)
        {
            _stringLocalizerFactory = services.GetRequiredServiceLazy<IStringLocalizerFactory>();
            _localizationOptions = services.GetRequiredServiceLazy<IOptions<MvcDataAnnotationsLocalizationOptions>>();
        }

        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            var displayMetadata = context.DisplayMetadata;

            if (displayMetadata.DisplayName != null)
            {
                return;
            }

            var attributes = context.Attributes;
            var displayAttribute = attributes.OfType<DisplayAttribute>().FirstOrDefault();
            var displayNameAttribute = attributes.OfType<DisplayNameAttribute>().FirstOrDefault();

            if (displayAttribute != null || displayNameAttribute != null)
            {
                return;
            }

            var containerType = context.Key.ContainerType ?? context.Key.ModelType;
            var localizer = _localizationOptions.Value.Value.DataAnnotationLocalizerProvider(containerType, _stringLocalizerFactory.Value);
            if (context.Key.Name == "BirthDate")
            {
                displayMetadata.DisplayName = () => localizer[context.Key.Name];
            }
        }
    }
}