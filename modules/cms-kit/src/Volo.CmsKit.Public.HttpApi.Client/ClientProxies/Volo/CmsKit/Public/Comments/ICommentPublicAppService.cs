// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.CmsKit.Public.Comments;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Public.Comments;

public interface ICommentPublicAppService : IApplicationService
{
    Task<ListResultDto<CommentWithDetailsDto>> GetListAsync(string entityType, string entityId);

    Task<CommentDto> CreateAsync(string entityType, string entityId, CreateCommentInput input);

    Task<CommentDto> UpdateAsync(Guid id, UpdateCommentInput input);

    Task DeleteAsync(Guid id);
}
