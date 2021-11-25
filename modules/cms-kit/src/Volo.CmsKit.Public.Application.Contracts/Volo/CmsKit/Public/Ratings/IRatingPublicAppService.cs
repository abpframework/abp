using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Public.Ratings;

public interface IRatingPublicAppService : IApplicationService
{
    Task<RatingDto> CreateAsync(string entityType, string entityId, CreateUpdateRatingInput input);

    Task DeleteAsync(string entityType, string entityId);

    Task<List<RatingWithStarCountDto>> GetGroupedStarCountsAsync(string entityType, string entityId);
}
