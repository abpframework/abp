using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Public.Comments
{
    public interface ICommentPublicAppService : IApplicationService
    {
        Task<ListResultDto<CommentWithDetailsDto>> GetAllForEntityAsync(string entityType, string entityId);

        Task<CommentDto> CreateAsync(CreateCommentInput input);

        Task<CommentDto> UpdateAsync(Guid id, UpdateCommentInput input);

        Task DeleteAsync(Guid id);
    }
}
