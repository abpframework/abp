using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Application.Services;

namespace Volo.Abp.FeatureManagement
{
    public interface IFeatureAppService : IApplicationService
    {
        Task<FeatureListDto> GetAsync([NotNull] string providerName, [NotNull] string providerKey); 

        Task UpdateAsync([NotNull] string providerName, [NotNull] string providerKey, UpdateFeaturesDto input); 
    }
}
