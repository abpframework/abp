using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Public.Comments
{
    [RequiresGlobalFeature(typeof(CommentsFeature))]
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
        public Task<ListResultDto<CommentWithDetailsDto>> GetListAsync(string entityType, string entityId)
        {
            return CommentPublicAppService.GetListAsync(entityType, entityId);
        }

        [HttpPost]
        [Route("{entityType}/{entityId}")]
        public Task<CommentDto> CreateAsync(string entityType, string entityId, CreateCommentInput input)
        {
            return CommentPublicAppService.CreateAsync(entityType, entityId, input);
        }

        [HttpPut]
        [Route("{id}")]
        public Task<CommentDto> UpdateAsync(Guid id, UpdateCommentInput input)
        {
            return CommentPublicAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return CommentPublicAppService.DeleteAsync(id);
        }
    }
}
