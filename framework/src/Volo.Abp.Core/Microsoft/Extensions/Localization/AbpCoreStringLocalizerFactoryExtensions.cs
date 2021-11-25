namespace Microsoft.Extensions.Localization;

public static class AbpCoreStringLocalizerFactoryExtensions
{
    public static IStringLocalizer Create<TResource>(this IStringLocalizerFactory localizerFactory)
    {
        return localizerFactory.Create(typeof(TResource));
    }
}
