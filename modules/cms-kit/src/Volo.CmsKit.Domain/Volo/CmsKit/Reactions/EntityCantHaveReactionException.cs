
using JetBrains.Annotations;
using System.Runtime.Serialization;
using Volo.Abp;

namespace Volo.CmsKit.Reactions
{
    public class EntityCantHaveReactionException : BusinessException
    {
        public EntityCantHaveReactionException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context)
        {
        }

        public EntityCantHaveReactionException([NotNull] string entityType)
        {
            EntityType = Check.NotNullOrEmpty(entityType, nameof(entityType));
            Code = CmsKitErrorCodes.Reactions.EntityCantHaveReaction;
            WithData(nameof(EntityType), EntityType);
        }

        public string EntityType { get; }
    }
}
