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
}
