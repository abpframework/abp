using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc;

public class AbpDataAnnotationAutoLocalizationMetadataDetailsProvider : IDisplayMetadataProvider
{
    private const string PropertyLocalizationKeyPrefix = "DisplayName:";

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

        if (attributes.OfType<DisplayAttribute>().Any() ||
            attributes.OfType<DisplayNameAttribute>().Any())
        {
            return;
        }

        if (context.Key.Name.IsNullOrWhiteSpace())
        {
            return;
        }

        if (_localizationOptions.Value.Value.DataAnnotationLocalizerProvider == null)
        {
            return;
        }

        var containerType = context.Key.ContainerType ?? context.Key.ModelType;
        var localizer = _localizationOptions.Value.Value.DataAnnotationLocalizerProvider(containerType, _stringLocalizerFactory.Value);

        displayMetadata.DisplayName = () =>
        {
                /*
                 * DisplayName:ClassName:PropertyName
                 * DisplayName:PropertyName
                 * ClassName:PropertyName
                 * PropertyName
                 */

            LocalizedString localizedString = null;

            if (context.Key.ContainerType != null)
            {
                localizedString = localizer[PropertyLocalizationKeyPrefix + context.Key.ContainerType.Name + ":" + context.Key.Name];
            }

            if (localizedString == null || localizedString.ResourceNotFound)
            {
                localizedString = localizer[PropertyLocalizationKeyPrefix + context.Key.Name];
            }

            if (localizedString.ResourceNotFound && context.Key.ContainerType != null)
            {
                localizedString = localizer[context.Key.ContainerType.Name + ":" + context.Key.Name];
            }

            if (localizedString.ResourceNotFound)
            {
                localizedString = localizer[context.Key.Name];
            }

            return localizedString;
        };
    }
}
