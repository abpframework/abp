using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Localization.External;

public interface IExternalLocalizationStore
{
    [CanBeNull] 
    LocalizationResourceBase GetResourceOrNull([NotNull] string resourceName);
    
    [ItemCanBeNull] 
    Task<LocalizationResourceBase> GetResourceOrNullAsync([NotNull] string resourceName);
    
    Task<string[]> GetResourceNamesAsync();
    
    Task<LocalizationResourceBase[]> GetResourcesAsync();
}