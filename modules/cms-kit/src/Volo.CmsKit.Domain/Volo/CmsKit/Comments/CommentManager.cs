
using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Comments
{
    public class CommentManager : DomainService
    {
        protected ICommentEntityTypeDefinitionStore DefinitionStore { get; }

        public CommentManager(ICommentEntityTypeDefinitionStore definitionStore)
        {
            DefinitionStore = definitionStore;
        }

        public virtual async Task<Comment> CreateAsync([NotNull] CmsUser creator,
                                                       [NotNull] string entityType,
                                                       [NotNull] string entityId,
                                                       [NotNull] string text,
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

            return new Comment(
                    GuidGenerator.Create(),
                    entityType,
                    entityId,
                    text,
                    repliedCommentId,
                    creator.Id,
                    CurrentTenant.Id);
        }
    }
}
