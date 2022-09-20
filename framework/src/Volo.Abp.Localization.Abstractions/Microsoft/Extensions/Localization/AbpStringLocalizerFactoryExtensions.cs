namespace Microsoft.Extensions.Localization;

public static class AbpStringLocalizerFactoryExtensions
{
    public static IStringLocalizer CreateDefaultOrNull(this IStringLocalizerFactory localizerFactory)
    {
        return (localizerFactory as IAbpStringLocalizerFactoryWithDefaultResourceSupport)
            ?.CreateDefaultOrNull();
    }
}
