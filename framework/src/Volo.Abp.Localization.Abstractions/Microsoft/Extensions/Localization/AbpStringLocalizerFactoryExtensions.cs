using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;

namespace Microsoft.Extensions.Localization;

public static class AbpStringLocalizerFactoryExtensions
{
    [CanBeNull]
    public static IStringLocalizer CreateDefaultOrNull(this IStringLocalizerFactory localizerFactory)
    {
        return (localizerFactory as IAbpStringLocalizerFactory)
            ?.CreateDefaultOrNull();
    }

    [CanBeNull]
    public static IStringLocalizer CreateByResourceNameOrNull(
        this IStringLocalizerFactory localizerFactory,
        string resourceName)
    {
        return (localizerFactory as IAbpStringLocalizerFactory)
            ?.CreateByResourceNameOrNull(resourceName);
    }
    
    [NotNull]
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
    
    [ItemCanBeNull]
    public static async Task<IStringLocalizer> CreateByResourceNameOrNullAsync(
        this IStringLocalizerFactory localizerFactory,
        string resourceName)
    {
        var abpLocalizerFactory = localizerFactory as IAbpStringLocalizerFactory;
        if (abpLocalizerFactory == null)
        {
            return null;
        } 
        
        return await abpLocalizerFactory.CreateByResourceNameOrNullAsync(resourceName);
    }
    
    [NotNull]
    public async static Task<IStringLocalizer> CreateByResourceNameAsync(
        this IStringLocalizerFactory localizerFactory,
        string resourceName)
    {
        var localizer = await localizerFactory.CreateByResourceNameOrNullAsync(resourceName);
        if (localizer == null)
        {
            throw new AbpException("Couldn't find a localizer with given resource name: " + resourceName);
        }
        
        return localizer;
    }
}
