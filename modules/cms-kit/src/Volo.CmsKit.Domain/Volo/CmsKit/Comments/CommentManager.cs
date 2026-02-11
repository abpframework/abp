using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.SettingManagement;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Comments;

public class CommentManager : DomainService
{
    protected ICommentEntityTypeDefinitionStore DefinitionStore { get; }
    protected ISettingManager SettingManager { get; }

    public CommentManager(
        ICommentEntityTypeDefinitionStore definitionStore, 
        ISettingManager settingManager)
    {
        DefinitionStore = definitionStore;
        SettingManager = settingManager;
    }

    public virtual async Task<Comment> CreateAsync([NotNull] CmsUser creator,
        [NotNull] string entityType,
        [NotNull] string entityId,
        [NotNull] string text,
        [CanBeNull] string url = null,
        [CanBeNull] Guid? repliedCommentId = null)
    {
        Check.NotNull(creator, nameof(creator));
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType), CommentConsts.MaxEntityTypeLength);
        Check.NotNullOrWhiteSpace(entityId, nameof(entityId), CommentConsts.MaxEntityIdLength);
        Check.NotNullOrWhiteSpace(text, nameof(text), CommentConsts.MaxTextLength);

        if (!await DefinitionStore.IsDefinedAsync(entityType))
        {
            throw new EntityNotCommentableException(entityType);
        }

        var comment =  new Comment(
            GuidGenerator.Create(),
            entityType,
            entityId,
            text,
            repliedCommentId,
            creator.Id,
            url,
            CurrentTenant.Id);
        
        
        var isRequireApprovementEnabled = bool.Parse(await SettingManager.GetOrNullGlobalAsync(CmsKitSettings.Comments.RequireApprovement));
        if (isRequireApprovementEnabled)
        {
            comment.WaitForApproval();
        }
        else
        {
            comment.Approve();
        }

        return comment;
    }
}
