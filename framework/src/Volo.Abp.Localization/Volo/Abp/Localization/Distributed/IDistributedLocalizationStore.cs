using System.Threading.Tasks;

namespace Volo.Abp.Localization.Distributed;

public interface IDistributedLocalizationStore
{
    Task SaveAsync();
    
    Task<DistributedLocalizationData> GetAsync();

    Task<string[]> GetResourceNames();
}