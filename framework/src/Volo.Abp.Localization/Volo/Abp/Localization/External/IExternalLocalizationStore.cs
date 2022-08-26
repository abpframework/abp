using JetBrains.Annotations;

namespace Volo.Abp.Localization.External;

public interface IExternalLocalizationStore
{
    [CanBeNull] 
    LocalizationResourceBase GetResourceOrNull([NotNull] string resourceName);
}