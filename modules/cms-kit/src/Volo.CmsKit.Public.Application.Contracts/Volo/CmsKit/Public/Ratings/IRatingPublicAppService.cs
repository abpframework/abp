using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Public.Ratings
{
    public interface IRatingPublicAppService : IApplicationService
    {
        Task<ListResultDto<RatingDto>> GetListAsync(string entityType, string entityId);

        Task<RatingDto> CreateAsync(string entityType, string entityId, CreateRatingInput input);

        Task<RatingDto> UpdateAsync(Guid id, UpdateRatingInput input);

        Task DeleteAsync(Guid id);
    }
}