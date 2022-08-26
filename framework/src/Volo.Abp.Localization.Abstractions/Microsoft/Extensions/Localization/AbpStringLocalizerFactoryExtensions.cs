using Volo.Abp;

namespace Microsoft.Extensions.Localization;

public static class AbpStringLocalizerFactoryExtensions
{
    public static IStringLocalizer CreateDefaultOrNull(this IStringLocalizerFactory localizerFactory)
    {
        return (localizerFactory as IAbpStringLocalizerFactory)
            ?.CreateDefaultOrNull();
    }

    public static IStringLocalizer CreateByResourceNameOrNull(
        this IStringLocalizerFactory localizerFactory,
        string resourceName)
    {
        return (localizerFactory as IAbpStringLocalizerFactory)
            ?.CreateByResourceNameOrNull(resourceName);
    }
    
    public static IStringLocalizer CreateByResourceName(
        this IStringLocalizerFactory localizerFactory,
        string resourceName)
    {
        var localizer = localizerFactory.CreateByResourceNameOrNull(resourceName);
        if (localizer == null)
        {
            throw new AbpException("Couldn't find a localizer with given resource name: " + resourceName);
        }
        
        return localizer;
    }
}
