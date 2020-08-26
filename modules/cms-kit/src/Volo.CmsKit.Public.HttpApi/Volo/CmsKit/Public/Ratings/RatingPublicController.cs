using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Public.Ratings
{
    [RequiresGlobalFeature(typeof(RatingsFeature))]
    [RemoteService(Name = CmsKitPublicRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Route("api/cms-kit-public/ratings")]
    public class RatingPublicController : CmsKitPublicControllerBase, IRatingPublicAppService
    {
        protected IRatingPublicAppService RatingPublicAppService { get; }

        public RatingPublicController(IRatingPublicAppService ratingPublicAppService)
        {
            RatingPublicAppService = ratingPublicAppService;
        }
        
        [HttpGet]
        [Route("{entityType}/{entityId}")]
        public virtual Task<ListResultDto<RatingDto>> GetListAsync(string entityType, string entityId)
        {
            return RatingPublicAppService.GetListAsync(entityType, entityId);
        }

        [HttpPut]
        [Route("{entityType}/{entityId}")]
        public virtual Task<RatingDto> CreateAsync(string entityType, string entityId, CreateRatingInput input)
        {
            return RatingPublicAppService.CreateAsync(entityType, entityId, input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<RatingDto> UpdateAsync(Guid id, UpdateRatingInput input)
        {
            return RatingPublicAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return RatingPublicAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("{entityType}/{entityId}")]
        public Task<RatingDto> GetCurrentUserRatingAsync(string entityType, string entityId)
        {
            return RatingPublicAppService.GetCurrentUserRatingAsync(entityType, entityId);
        }
    }
}