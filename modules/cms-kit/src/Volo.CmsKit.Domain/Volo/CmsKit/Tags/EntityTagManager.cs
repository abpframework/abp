using JetBrains.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Volo.CmsKit.Tags
{
    public class EntityTagManager : DomainService, IEntityTagManager
    {
        protected IEntityTagRepository EntityTagRepository { get; }
        protected ITagDefinitionStore TagDefinitionStore { get; }

        public EntityTagManager(
            IEntityTagRepository entityTagRepository,
            ITagDefinitionStore tagDefinitionStore)
        {
            EntityTagRepository = entityTagRepository;
            TagDefinitionStore = tagDefinitionStore;
        }

        public virtual async Task<EntityTag> AddTagToEntityAsync(
            [NotNull] Guid tagId,
            [NotNull] string entityType,
            [NotNull] string entityId,
            [CanBeNull] Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            if (!await TagDefinitionStore.IsDefinedAsync(entityType))
            {
                throw new EntityNotTaggableException(entityType);
            }

            var entityTag = new EntityTag(tagId, entityId, tenantId);
            return await EntityTagRepository.InsertAsync(entityTag, cancellationToken: cancellationToken);
        }

        public virtual async Task RemoveTagFromEntityAsync(
            [NotNull] Guid tagId,
            [NotNull] string entityType,
            [NotNull] string entityId,
            [CanBeNull] Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var entityTag = await EntityTagRepository.FindAsync(tagId, entityId, tenantId, cancellationToken);
            await EntityTagRepository.DeleteAsync(entityTag, cancellationToken: cancellationToken);
        }
    }
}