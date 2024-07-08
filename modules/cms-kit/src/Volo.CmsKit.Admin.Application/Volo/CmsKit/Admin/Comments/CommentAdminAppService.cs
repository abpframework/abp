using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.SettingManagement;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Admin.Comments;

[RequiresFeature(CmsKitFeatures.CommentEnable)]
[RequiresGlobalFeature(typeof(CommentsFeature))]
[Authorize(CmsKitAdminPermissions.Comments.Default)]
public class CommentAdminAppService : CmsKitAdminAppServiceBase, ICommentAdminAppService
{
    protected ICommentRepository CommentRepository { get; }

    protected ISettingManager SettingManager { get; }

    public CommentAdminAppService(
        ICommentRepository commentRepository,
        ISettingManager settingManager)
    {
        CommentRepository = commentRepository;
        SettingManager = settingManager;
    }

    public virtual async Task<PagedResultDto<CommentWithAuthorDto>> GetListAsync(CommentGetListInput input)
    {
        var totalCount = await CommentRepository.GetCountAsync(
            input.Text,
            input.EntityType,
            input.RepliedCommentId,
            input.Author,
            input.CreationStartDate,
            input.CreationEndDate,
            input.CommentApproveState
        );

        var comments = await CommentRepository.GetListAsync(
            input.Text,
            input.EntityType,
            input.RepliedCommentId,
            input.Author,
            input.CreationStartDate,
            input.CreationEndDate,
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount,
            input.CommentApproveState
        );

        var dtos = comments.Select(queryResultItem =>
        {
            var dto = ObjectMapper.Map<Comment, CommentWithAuthorDto>(queryResultItem.Comment);
            dto.Author = ObjectMapper.Map<CmsUser, CmsUserDto>(queryResultItem.Author);

            return dto;
        }).ToList();

        return new PagedResultDto<CommentWithAuthorDto>(totalCount, dtos);
    }

    public virtual async Task<CommentWithAuthorDto> GetAsync(Guid id)
    {
        var comment = await CommentRepository.GetWithAuthorAsync(id);

        var dto = ObjectMapper.Map<Comment, CommentWithAuthorDto>(comment.Comment);
        dto.Author = ObjectMapper.Map<CmsUser, CmsUserDto>(comment.Author);

        return dto;
    }

    [Authorize(CmsKitAdminPermissions.Comments.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        var comment = await CommentRepository.GetAsync(id);
        await CommentRepository.DeleteWithRepliesAsync(comment);
    }

    [Authorize(CmsKitAdminPermissions.Comments.Update)]
    public async Task UpdateApprovalStatusAsync(Guid id, CommentApprovalDto input)
    {
        var comment = await CommentRepository.GetAsync(id);

        if (input.IsApproved)
        {
            comment.Approve();
        }
        else
        {
            comment.Reject();
        }

        await CommentRepository.UpdateAsync(comment);
    }

    [Authorize(CmsKitAdminPermissions.Comments.SettingManagement)]
    public async Task UpdateSettingsAsync(CommentSettingsDto input)
    {
        await SettingManager.SetGlobalAsync(CmsKitSettings.Comments.RequireApprovement, input.CommentRequireApprovement.ToString());
    }

    public async Task<int> GetWaitingCountAsync()
    {
        return (int)await CommentRepository.GetCountAsync(commentApproveState: CommentApproveState.Waiting);
    }
}