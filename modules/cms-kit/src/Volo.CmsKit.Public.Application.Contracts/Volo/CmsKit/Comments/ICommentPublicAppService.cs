using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Comments
{
    public interface ICommentPublicAppService : IApplicationService
    {
        Task<ListResultDto<CommentWithDetailsDto>> GetAllForEntityAsync(string entityType, string entityId);

        Task<CommentDto> CreateAsync(CreateCommentInput input);

        Task<CommentDto> UpdateAsync(Guid id, UpdateCommentInput input);

        Task DeleteAsync(Guid id);
    }
}
