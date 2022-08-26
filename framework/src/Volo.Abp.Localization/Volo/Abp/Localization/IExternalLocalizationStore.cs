using JetBrains.Annotations;

namespace Volo.Abp.Localization;

public interface IExternalLocalizationStore
{
    [CanBeNull] 
    LocalizationResourceBase GetResourceOrNull([NotNull] string resourceName);
}