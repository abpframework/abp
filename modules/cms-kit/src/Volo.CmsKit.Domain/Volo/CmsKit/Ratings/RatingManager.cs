using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Ratings;

public class RatingManager : DomainService
{
    protected IRatingRepository RatingRepository { get; }
    protected IRatingEntityTypeDefinitionStore RatingDefinitionStore { get; }

    public RatingManager(
        IRatingRepository ratingRepository,
        IRatingEntityTypeDefinitionStore ratingDefinitionStore)
    {
        RatingRepository = ratingRepository;
        RatingDefinitionStore = ratingDefinitionStore;
    }

    public async Task<Rating> SetStarAsync(CmsUser user, string entityType, string entityId, short starCount)
    {
        var currentUserRating = await RatingRepository.GetCurrentUserRatingAsync(entityType, entityId, user.Id);

        if (currentUserRating != null)
        {
            currentUserRating.SetStarCount(starCount);

            return await RatingRepository.UpdateAsync(currentUserRating);
        }

        if (!await RatingDefinitionStore.IsDefinedAsync(entityType))
        {
            throw new EntityCantHaveRatingException(entityType);
        }

        return await RatingRepository.InsertAsync(
            new Rating(
                GuidGenerator.Create(),
                entityType,
                entityId,
                starCount,
                user.Id,
                CurrentTenant.Id
            )
        );
    }
}
