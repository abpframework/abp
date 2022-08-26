using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Localization;

public class NullExternalLocalizationStore : IExternalLocalizationStore, ISingletonDependency
{
    public LocalizationResource GetResourceOrNull(string resourceName)
    {
        return null;
    }
}