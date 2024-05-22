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
using Volo.CmsKit.Settings;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Admin.Comments;

[RequiresFeature(CmsKitFeatures.CommentEnable)]
[RequiresGlobalFeature(typeof(CommentsFeature))]
[Authorize(CmsKitAdminPermissions.Comments.Default)]
public class CommentAdminAppService : CmsKitAdminAppServiceBase, ICommentAdminAppService
{
    protected ICommentRepository CommentRepository { get; }

    private readonly ISettingManager _settingManager;
    public CommentAdminAppService(ICommentRepository commentRepository, ISettingManager settingManager)
    {
        CommentRepository = commentRepository;
        _settingManager = settingManager;
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
				input.commentApproveStateType
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
            input.commentApproveStateType
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



    public async Task UpdateApprovalStatusAsync(Guid id, CommentApprovalDto commentApprovalDto)
    {
		var comment = await CommentRepository.GetAsync(id);
		comment.IsApproved = commentApprovalDto.IsApproved;

		await CommentRepository.UpdateAsync(comment);
	}

    public async Task SetSettings(SettingsDto settingsDto)
    {
        await _settingManager.SetGlobalAsync(AppSettings.RequireApprovement, settingsDto.RequireApprovement.ToString());
    }

    public async Task<SettingsDto> GetSettings()
    {
        string approvalSettingValue = await _settingManager.GetOrNullGlobalAsync(AppSettings.RequireApprovement);

        if (bool.TryParse(approvalSettingValue, out bool isApprovalRequired))
        {
            SettingsDto settings = new SettingsDto
            {
                RequireApprovement = isApprovalRequired
            };
            return settings;
        }
        return null;
    }
	public async Task<int> GetWaitingCommentCount()
	{
		var count = await CommentRepository.GetCountAsync(commentApproveStateType: CommentApproveStateType.Waiting);
		return (int)(count);

	}

    public async Task<PagedResultDto<CommentWithAuthorDto>> GetWaitingCommentsWithRepliesAsync(CommentGetListInput input)
    {
        var totalCount = await CommentRepository.GetCountAsync(
         input.Text,
         input.EntityType,
         input.RepliedCommentId,
         input.Author,
         input.CreationStartDate,
         input.CreationEndDate,
          CommentApproveStateType.Waiting
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
            CommentApproveStateType.Waiting
        );

        var dtos = comments.Select(queryResultItem =>
        {
            var dto = ObjectMapper.Map<Comment, CommentWithAuthorDto>(queryResultItem.Comment);
            dto.Author = ObjectMapper.Map<CmsUser, CmsUserDto>(queryResultItem.Author);

            return dto;
        }).ToList();

        return new PagedResultDto<CommentWithAuthorDto>(totalCount, dtos);
    }
}
