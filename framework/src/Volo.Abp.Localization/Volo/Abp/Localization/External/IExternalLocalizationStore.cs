using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Localization.Distributed;

namespace Volo.Abp.Localization.External;

public interface IExternalLocalizationStore
{
    Task SaveAsync();

    [CanBeNull] 
    LocalizationResourceBase GetResourceOrNull([NotNull] string resourceName);
    
    Task<ExternalLocalizationData> GetAsync();
    
    Task<string[]> GetResourceNames();
}