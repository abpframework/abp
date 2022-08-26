using JetBrains.Annotations;

namespace Volo.Abp.Localization;

public interface IExternalLocalizationStore
{
    [CanBeNull] 
    LocalizationResource GetResourceOrNull([NotNull] string resourceName);
}