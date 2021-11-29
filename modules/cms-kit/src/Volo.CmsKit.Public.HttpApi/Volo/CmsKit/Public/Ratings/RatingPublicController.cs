using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Public.Ratings
{
    [RequiresGlobalFeature(typeof(RatingsFeature))]
    [RemoteService(Name = CmsKitPublicRemoteServiceConsts.RemoteServiceName)]
    [Area(CmsKitPublicRemoteServiceConsts.ModuleName)]
    [Route("api/cms-kit-public/ratings")]
    public class RatingPublicController : CmsKitPublicControllerBase, IRatingPublicAppService
    {
        protected IRatingPublicAppService RatingPublicAppService { get; }

        public RatingPublicController(IRatingPublicAppService ratingPublicAppService)
        {
            RatingPublicAppService = ratingPublicAppService;
        }
        
        [HttpPut]
        [Route("{entityType}/{entityId}")]
        public virtual Task<RatingDto> CreateAsync(string entityType, string entityId, CreateUpdateRatingInput input)
        {
            return RatingPublicAppService.CreateAsync(entityType, entityId, input);
        }

        [HttpDelete]
        [Route("{entityType}/{entityId}")]
        public virtual Task DeleteAsync(string entityType, string entityId)
        {
            return RatingPublicAppService.DeleteAsync(entityType, entityId);
        }

        [HttpGet]
        [Route("{entityType}/{entityId}")]
        public virtual Task<List<RatingWithStarCountDto>> GetGroupedStarCountsAsync(string entityType, string entityId)
        {
            return RatingPublicAppService.GetGroupedStarCountsAsync(entityType, entityId);
        }
    }
}
