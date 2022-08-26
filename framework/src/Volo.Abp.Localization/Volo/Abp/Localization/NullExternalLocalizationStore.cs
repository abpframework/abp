using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Localization;

public class NullExternalLocalizationStore : IExternalLocalizationStore, ISingletonDependency
{
    public LocalizationResourceBase GetResourceOrNull(string resourceName)
    {
        return null;
    }
}