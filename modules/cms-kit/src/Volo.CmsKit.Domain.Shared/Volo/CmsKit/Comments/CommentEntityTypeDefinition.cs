using JetBrains.Annotations;
using Volo.Abp;

namespace Volo.CmsKit.Comments;

public class CommentEntityTypeDefinition : EntityTypeDefinition
{
    public CommentEntityTypeDefinition([NotNull] string entityType) : base(entityType)
    {
        EntityType = Check.NotNullOrEmpty(entityType, nameof(entityType));
    }
}
