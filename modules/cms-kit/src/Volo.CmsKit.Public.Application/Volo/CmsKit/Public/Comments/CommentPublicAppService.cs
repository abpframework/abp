using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.SettingManagement;
using Volo.Abp.Users;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Public.Comments;

[RequiresFeature(CmsKitFeatures.CommentEnable)]
[RequiresGlobalFeature(typeof(CommentsFeature))]
public class CommentPublicAppService : CmsKitPublicAppServiceBase, ICommentPublicAppService
{
    protected string RegexUrlPattern =
        @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()!@:%_\+.~#?&\/\/=]*)";
    
    protected ICommentRepository CommentRepository { get; }
    protected ICmsUserLookupService CmsUserLookupService { get; }
    public IDistributedEventBus DistributedEventBus { get; }
    protected CommentManager CommentManager { get; }
    protected CmsKitCommentOptions CmsCommentOptions { get; }

    private readonly ISettingManager SettingManager;

    public CommentPublicAppService(
        ICommentRepository commentRepository,
        ICmsUserLookupService cmsUserLookupService,
        IDistributedEventBus distributedEventBus,
        CommentManager commentManager,
        IOptionsSnapshot<CmsKitCommentOptions> cmsCommentOptions,
        ISettingManager settingManager
        )
    {
        CommentRepository = commentRepository;
        CmsUserLookupService = cmsUserLookupService;
        DistributedEventBus = distributedEventBus;
        CommentManager = commentManager;
        CmsCommentOptions = cmsCommentOptions.Value;
        SettingManager = settingManager;
    }

    public virtual async Task<ListResultDto<CommentWithDetailsDto>> GetListAsync(string entityType, string entityId)
    {
        var isRequireApprovementEnabled = bool.Parse(await SettingManager.GetOrNullGlobalAsync(CmsKitSettings.Comments.RequireApprovement));
        
        var commentsWithAuthor = isRequireApprovementEnabled
            ? await CommentRepository.GetListWithAuthorsAsync(entityType, entityId, CommentApproveState.Approved)
            : await CommentRepository.GetListWithAuthorsAsync(entityType, entityId, CommentApproveState.Approved | CommentApproveState.Waiting);


        return new ListResultDto<CommentWithDetailsDto>(
            ConvertCommentsToNestedStructure(commentsWithAuthor)
        );
    }

    [Authorize]
    public virtual async Task<CommentDto> CreateAsync(string entityType, string entityId, CreateCommentInput input)
    {
        CheckExternalUrls(entityType, input.Text);

        if (input.RepliedCommentId.HasValue)
        {
            await CommentRepository.GetAsync(input.RepliedCommentId.Value);
        }

        await CheckIdempotencyTokenUniquenessAsync(input.IdempotencyToken);

        var user = await CmsUserLookupService.GetByIdAsync(CurrentUser.GetId());
        var comment = await CommentRepository.InsertAsync(
            await CommentManager.CreateAsync(
                user,
                entityType,
                entityId,
                input.Text,
                input.Url,
                input.RepliedCommentId
            )
        );

        await UnitOfWorkManager.Current.SaveChangesAsync();

        await DistributedEventBus.PublishAsync(new CreatedCommentEvent
        {
            Id = comment.Id
        });

        return ObjectMapper.Map<Comment, CommentDto>(comment);
    }

    [Authorize]
    public virtual async Task<CommentDto> UpdateAsync(Guid id, UpdateCommentInput input)
    {
        var comment = await CommentRepository.GetAsync(id);
        if (comment.CreatorId != CurrentUser.GetId())
        {
            throw new AbpAuthorizationException();
        }
        
        CheckExternalUrls(comment.EntityType, input.Text);

        comment.SetText(input.Text);
        comment.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        var updatedComment = await CommentRepository.UpdateAsync(comment);

        return ObjectMapper.Map<Comment, CommentDto>(updatedComment);
    }

    [Authorize]
    public virtual async Task DeleteAsync(Guid id)
    {
        var allowDelete = await AuthorizationService.IsGrantedAsync(CmsKitPublicPermissions.Comments.DeleteAll);

        var comment = await CommentRepository.GetAsync(id);
        if (allowDelete || comment.CreatorId == CurrentUser.Id)
        {
            await CommentRepository.DeleteWithRepliesAsync(comment);
        }
        else
        {
            throw new AbpAuthorizationException();
        }
    }
    
    protected virtual void CheckExternalUrls(string entityType, string text)
    {
        if (!CmsCommentOptions.AllowedExternalUrls.TryGetValue(entityType, out var allowedExternalUrls))
        {
            return;
        }

        text = text.Replace("www.", "https://www.").Replace("://https", "");

        var matches = Regex.Matches(text, RegexUrlPattern,
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        foreach (Match match in matches)
        {
            if (!match.Success || match.Groups.Count <= 0)
            {
                continue;
            }

            var normalizedFullUrl = NormalizeUrl(match.Groups[0].Value);
            
            if (!allowedExternalUrls.Any(allowedExternalUrl =>
                    normalizedFullUrl.Contains(NormalizeUrl(allowedExternalUrl), StringComparison.OrdinalIgnoreCase)))
            {
                throw new UserFriendlyException(L["UnAllowedExternalUrlMessage"]);
            }
        }
    }
    
    private static string NormalizeUrl(string url)
    {
        return url.Replace("www.", "").RemovePostFix("/");
    }
    
    private List<CommentWithDetailsDto> ConvertCommentsToNestedStructure(List<CommentWithAuthorQueryResultItem> comments)
    {
        //TODO: I think this method can be optimized if you use dictionaries instead of straight search

        var parentComments = comments
            .Where(c => c.Comment.RepliedCommentId == null)
            .Select(c => ObjectMapper.Map<Comment, CommentWithDetailsDto>(c.Comment))
            .ToList();

        foreach (var parentComment in parentComments)
        {
            parentComment.Author = GetAuthorAsDtoFromCommentList(comments, parentComment.Id);

            parentComment.Replies = comments
                .Where(c => c.Comment.RepliedCommentId == parentComment.Id)
                .Select(c => ObjectMapper.Map<Comment, CommentDto>(c.Comment))
                .ToList();

            foreach (var reply in parentComment.Replies)
            {
                reply.Author = GetAuthorAsDtoFromCommentList(comments, reply.Id);
            }
        }

        return parentComments;
    }

    private CmsUserDto GetAuthorAsDtoFromCommentList(List<CommentWithAuthorQueryResultItem> comments, Guid commentId)
    {
        return ObjectMapper.Map<CmsUser, CmsUserDto>(comments.Single(c => c.Comment.Id == commentId).Author);
    }

    private async Task CheckIdempotencyTokenUniquenessAsync(string idempotencyToken) 
    {
        if(!await CommentRepository.ExistsAsync(idempotencyToken))
        {
            return;
        }

        throw new UserFriendlyException(L["DuplicateCommentAttemptMessage"]);
    }
}
