using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.FeatureManagement
{
    [RemoteService]
    [Area("abp")]
    public class FeaturesController : AbpController, IFeatureAppService
    {
        private readonly IFeatureAppService _featureAppService;

        public FeaturesController(IFeatureAppService featureAppService)
        {
            _featureAppService = featureAppService;
        }

        public Task<FeatureListDto> GetAsync(string providerName, string providerKey)
        {
            return _featureAppService.GetAsync(providerName, providerKey);
        }

        public Task UpdateAsync(string providerName, string providerKey, UpdateFeaturesDto input)
        {
            return _featureAppService.UpdateAsync(providerName, providerKey, input);
        }
    }
}