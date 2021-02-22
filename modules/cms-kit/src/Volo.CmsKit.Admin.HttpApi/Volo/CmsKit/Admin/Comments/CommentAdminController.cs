using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Comments
{
    [Authorize(CmsKitAdminPermissions.Comments.Default)]
    [RequiresGlobalFeature(typeof(CommentsFeature))]
    [RemoteService(Name = CmsKitCommonRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Route("api/cms-kit-admin/comments")]
    public class CommentAdminController : CmsKitAdminController, ICommentAdminAppService
    {
        protected ICommentAdminAppService CommentAdminAppService { get; }

        public CommentAdminController(ICommentAdminAppService commentAdminAppService)
        {
            CommentAdminAppService = commentAdminAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<CommentWithAuthorDto>> GetListAsync(CommentGetListInput input)
        {
            return CommentAdminAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<CommentWithAuthorDto> GetAsync(Guid id)
        {
            return CommentAdminAppService.GetAsync(id);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(CmsKitAdminPermissions.Comments.Delete)]
        public virtual Task DeleteAsync(Guid id)
        {
            return CommentAdminAppService.DeleteAsync(id);
        }
    }
}