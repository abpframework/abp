using JetBrains.Annotations;
using System;
using Volo.Abp;

namespace Volo.CmsKit.Comments
{
    public class CommentEntityTypeDefinition : IEquatable<CommentEntityTypeDefinition>
    {
        public CommentEntityTypeDefinition([NotNull] string entityType)
        {
            EntityType = Check.NotNullOrEmpty(entityType, nameof(entityType));
        }

        public string EntityType { get; }

        public bool Equals(CommentEntityTypeDefinition other)
        {
            return EntityType == other?.EntityType;
        }
    }
}
