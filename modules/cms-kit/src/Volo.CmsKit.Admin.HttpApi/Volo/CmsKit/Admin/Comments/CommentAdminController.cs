using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Comments;

[RequiresFeature(CmsKitFeatures.CommentEnable)]
[Authorize(CmsKitAdminPermissions.Comments.Default)]
[RequiresGlobalFeature(typeof(CommentsFeature))]
[RemoteService(Name = CmsKitAdminRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitAdminRemoteServiceConsts.ModuleName)]
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

    [HttpPut]
    [Route("{id}/approval-status")]
    public Task UpdateApprovalStatusAsync(Guid id, CommentApprovalDto input)
    {
        return CommentAdminAppService.UpdateApprovalStatusAsync(id, input);
    }

    [HttpPost]
    [Route("settings")]
    public Task UpdateSettingsAsync(CommentSettingsDto input)
    {
       return CommentAdminAppService.UpdateSettingsAsync(input);
    }

    [HttpGet]
    [Route("waiting-count")]
    public Task<int> GetWaitingCountAsync()
    {
        return CommentAdminAppService.GetWaitingCountAsync();
    }
}
