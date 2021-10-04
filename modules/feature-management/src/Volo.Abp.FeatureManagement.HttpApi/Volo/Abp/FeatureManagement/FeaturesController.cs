using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.FeatureManagement
{
    [RemoteService(Name = FeatureManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("featureManagement")]
    [Route("api/feature-management/features")]
    public class FeaturesController : AbpControllerBase, IFeatureAppService
    {
        protected IFeatureAppService FeatureAppService { get; }

        public FeaturesController(IFeatureAppService featureAppService)
        {
            FeatureAppService = featureAppService;
        }

        [HttpGet]
        public virtual Task<GetFeatureListResultDto> GetAsync(string providerName, string providerKey)
        {
            return FeatureAppService.GetAsync(providerName, providerKey);
        }

        [HttpPut]
        public virtual Task UpdateAsync(string providerName, string providerKey, UpdateFeaturesDto input)
        {
            return FeatureAppService.UpdateAsync(providerName, providerKey, input);
        }
    }
}
