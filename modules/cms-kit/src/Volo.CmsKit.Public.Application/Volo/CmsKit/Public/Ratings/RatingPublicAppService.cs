using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Authorization;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Users;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Public.Ratings
{
    [RequiresGlobalFeature(typeof(RatingsFeature))]
    public class RatingPublicAppService : CmsKitPublicAppServiceBase, IRatingPublicAppService
    {
        protected IRatingRepository RatingRepository { get; }
        public ICmsUserLookupService CmsUserLookupService { get; }
        protected RatingManager RatingManager { get; }

        public RatingPublicAppService(
            IRatingRepository ratingRepository,
            ICmsUserLookupService cmsUserLookupService,
            RatingManager ratingManager)
        {
            RatingRepository = ratingRepository;
            CmsUserLookupService = cmsUserLookupService;
            RatingManager = ratingManager;
        }

        [Authorize]
        public virtual async Task<RatingDto> CreateAsync(string entityType, string entityId,
            CreateUpdateRatingInput input)
        {
            var userId = CurrentUser.GetId();
            var user = await CmsUserLookupService.GetByIdAsync(userId);

            var rating = await RatingManager.SetStarAsync(user, entityType, entityId, input.StarCount);

            return ObjectMapper.Map<Rating, RatingDto>(rating);
        }

        [Authorize]
        public virtual async Task DeleteAsync(string entityType, string entityId)
        {
            var rating = await RatingRepository.GetCurrentUserRatingAsync(entityType, entityId, CurrentUser.GetId());

            if (rating.CreatorId != CurrentUser.GetId())
            {
                throw new AbpAuthorizationException();
            }

            await RatingRepository.DeleteAsync(rating.Id);
        }

        public virtual async Task<List<RatingWithStarCountDto>> GetGroupedStarCountsAsync(string entityType,
            string entityId)
        {
            var ratings = await RatingRepository.GetGroupedStarCountsAsync(entityType, entityId);

            var userRatingOrNull = CurrentUser.IsAuthenticated
                ? await RatingRepository.GetCurrentUserRatingAsync(entityType, entityId, CurrentUser.GetId())
                : null;

            var ratingWithStarCountDto = new List<RatingWithStarCountDto>();

            foreach (var rating in ratings)
            {
                ratingWithStarCountDto.Add(
                    new RatingWithStarCountDto
                    {
                        StarCount = rating.StarCount,
                        Count = rating.Count,
                        IsSelectedByCurrentUser = userRatingOrNull != null && userRatingOrNull.StarCount == rating.StarCount
                    });
            }

            return ratingWithStarCountDto;
        }
    }
}