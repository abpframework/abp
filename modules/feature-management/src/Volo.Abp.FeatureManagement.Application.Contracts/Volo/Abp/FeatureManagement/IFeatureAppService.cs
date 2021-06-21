using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Application.Services;

namespace Volo.Abp.FeatureManagement
{
    public interface IFeatureAppService : IApplicationService
    {
        Task<GetFeatureListResultDto> GetAsync([NotNull] string providerName, string providerKey); 

        Task UpdateAsync([NotNull] string providerName, string providerKey, UpdateFeaturesDto input);
    }
}
