using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Users;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Public.Ratings
{
    public class RatingPublicAppService : ApplicationService, IRatingPublicAppService
    {
        protected IRatingRepository RatingRepository { get; }
        public ICmsUserLookupService CmsUserLookupService { get; }

        public RatingPublicAppService(IRatingRepository ratingRepository, ICmsUserLookupService cmsUserLookupService)
        {
            RatingRepository = ratingRepository;
            CmsUserLookupService = cmsUserLookupService;
        }

        [Authorize]
        public virtual async Task<RatingDto> CreateAsync(string entityType, string entityId,
            CreateUpdateRatingInput input)
        {
            var userId = CurrentUser.GetId();
            var user = await CmsUserLookupService.GetByIdAsync(userId);

            var currentUserRating = await RatingRepository.GetCurrentUserRatingAsync(entityType, entityId, userId);

            if (currentUserRating != null)
            {
                currentUserRating.SetStarCount(input.StarCount);
                var updatedRating = await RatingRepository.UpdateAsync(currentUserRating);

                return ObjectMapper.Map<Rating, RatingDto>(updatedRating);
            }

            var rating = await RatingRepository.InsertAsync(
                new Rating(
                    GuidGenerator.Create(),
                    entityType,
                    entityId,
                    input.StarCount,
                    user.Id,
                    CurrentTenant.Id
                )
            );

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