using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.FeatureManagement
{
    [RemoteService(Name = FeatureManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("abp")]
    public class FeaturesController : AbpController, IFeatureAppService
    {
        protected IFeatureAppService FeatureAppService { get; }

        public FeaturesController(IFeatureAppService featureAppService)
        {
            FeatureAppService = featureAppService;
        }

        public virtual Task<FeatureListDto> GetAsync(string providerName, string providerKey)
        {
            return FeatureAppService.GetAsync(providerName, providerKey);
        }

        public virtual Task UpdateAsync(string providerName, string providerKey, UpdateFeaturesDto input)
        {
            return FeatureAppService.UpdateAsync(providerName, providerKey, input);
        }
    }
}