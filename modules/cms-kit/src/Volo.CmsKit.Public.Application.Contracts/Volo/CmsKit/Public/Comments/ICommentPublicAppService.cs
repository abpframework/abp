using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Public.Comments;

public interface ICommentPublicAppService : IApplicationService
{
    Task<ListResultDto<CommentWithDetailsDto>> GetListAsync(string entityType, string entityId);

    Task<CommentDto> CreateAsync(string entityType, string entityId, CreateCommentInput input);

    Task<CommentDto> UpdateAsync(Guid id, UpdateCommentInput input);

    Task DeleteAsync(Guid id);
}
