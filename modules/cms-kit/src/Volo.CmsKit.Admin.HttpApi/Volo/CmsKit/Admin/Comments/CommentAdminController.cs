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

	[HttpPost]
	[Route("{id}")]
    [Authorize(CmsKitAdminPermissions.Comments.Update)]
    public Task UpdateApprovalStatusAsync(Guid id, CommentApprovalDto input)
    {
		return CommentAdminAppService.UpdateApprovalStatusAsync(id, input);

	}

    [HttpPost]
    [Route("settings")]
    [Authorize(CmsKitAdminPermissions.Comments.Default)]
    public Task SetSettingsAsync(SettingsDto input)
    {
       return CommentAdminAppService.SetSettingsAsync(input);
    }

    [HttpGet]
    [Route("settings")]
    [Authorize(CmsKitAdminPermissions.Comments.Default)]
    public Task<SettingsDto> GetSettingsAsync()
    {
       return CommentAdminAppService.GetSettingsAsync();
    }

	[HttpGet]
	[Route("waiting-count")]
    [Authorize(CmsKitAdminPermissions.Comments.Default)]
    public Task<int> GetWaitingCountAsync()
	{
		return CommentAdminAppService.GetWaitingCountAsync();
	}

    [HttpGet]
    [Route("waiting")]
    [Authorize(CmsKitAdminPermissions.Comments.Default)]
    public Task<PagedResultDto<CommentWithAuthorDto>> GetWaitingWithRepliesAsync(CommentGetListInput input)
    {
        return CommentAdminAppService.GetWaitingWithRepliesAsync(input);
    }
}
