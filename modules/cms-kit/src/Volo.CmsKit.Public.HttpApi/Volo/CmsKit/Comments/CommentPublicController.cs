using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Comments
{
    [RemoteService(Name = CmsKitPublicRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Route("api/cms-kit-public/comments")]
    public class CommentPublicController :  CmsKitPublicControllerBase, ICommentPublicAppService
    {
        public ICommentPublicAppService CommentPublicAppService { get; }

        public CommentPublicController(ICommentPublicAppService commentPublicAppService)
        {
            CommentPublicAppService = commentPublicAppService;
        }

        [HttpGet]
        [Route("{entityType}/{entityId}")]
        public Task<ListResultDto<CommentWithDetailsDto>> GetAllForEntityAsync(string entityType, string entityId)
        {
            return CommentPublicAppService.GetAllForEntityAsync(entityType, entityId);
        }

        [HttpPost]
        public Task<CommentDto> CreateAsync(CreateCommentInput input)
        {
            return CommentPublicAppService.CreateAsync(input);
        }

        [HttpPost]
        [Route("{id}")]
        public Task<CommentDto> UpdateAsync(Guid id, UpdateCommentInput input)
        {
            return CommentPublicAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("update")]
        public Task DeleteAsync(Guid id)
        {
            return CommentPublicAppService.DeleteAsync(id);
        }
    }
}
