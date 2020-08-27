using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
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

        public virtual async Task<ListResultDto<RatingDto>> GetListAsync(string entityType, string entityId)
        {
            var ratings = await RatingRepository.GetListAsync(entityType, entityId);
            var ratingDto = ObjectMapper.Map<List<Rating>, List<RatingDto>>(ratings);

            return new ListResultDto<RatingDto>(ratingDto);
        }

        [Authorize]
        public virtual async Task<RatingDto> CreateAsync(string entityType, string entityId, CreateRatingInput input)
        {
            var user = await CmsUserLookupService.GetByIdAsync(CurrentUser.GetId());

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
        public virtual async Task<RatingDto> UpdateAsync(Guid id, UpdateRatingInput input)
        {
            var rating = await RatingRepository.GetAsync(id);

            if (rating.CreatorId != CurrentUser.GetId())
            {
                throw new AbpAuthorizationException();
            }
            
            rating.SetStarCount(input.StarCount);

            var updatedRating = await RatingRepository.UpdateAsync(rating);

            return ObjectMapper.Map<Rating, RatingDto>(updatedRating);
        }

        [Authorize]
        public virtual async Task DeleteAsync(Guid id)
        {
            var rating = await RatingRepository.GetAsync(id);

            if (rating.CreatorId != CurrentUser.GetId())
            {
                throw new AbpAuthorizationException();
            }

            await RatingRepository.DeleteAsync(id);
        }

        [Authorize]
        public virtual async Task<RatingDto> GetCurrentUserRatingAsync(string entityType, string entityId)
        {
            var currentUserId = CurrentUser.GetId();
            
            var rating = await RatingRepository.GetCurrentUserRatingAsync(entityType, entityId, currentUserId);

            return ObjectMapper.Map<Rating, RatingDto>(rating);
        }

        public virtual async Task<List<RatingWithStarCountDto>> GetGroupedStarCountsAsync(string entityType, string entityId)
        {
            var ratings = await RatingRepository.GetGroupedStarCountsAsync(entityType, entityId);

            return ObjectMapper.Map<List<RatingWithStarCountQueryResultItem>, List<RatingWithStarCountDto>>(ratings);
        }
    }
}