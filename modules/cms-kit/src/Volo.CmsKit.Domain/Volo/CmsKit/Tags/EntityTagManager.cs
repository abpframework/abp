using JetBrains.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Tags
{
    public class EntityTagManager : DomainService, IEntityTagManager
    {
        protected readonly IEntityTagRepository _entityTagRepository;
        protected readonly ITagDefinitionStore _tagDefinitionStore;

        public EntityTagManager(
            IEntityTagRepository entityTagRepository,
            ITagDefinitionStore tagDefinitionStore)
        {
            _entityTagRepository = entityTagRepository;
            _tagDefinitionStore = tagDefinitionStore;
        }

        public async Task<EntityTag> AddTagToEntityAsync(
            [NotNull] Guid tagId,
            [NotNull] string entityType,
            [NotNull] string entityId,
            [CanBeNull] Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            if (!await _tagDefinitionStore.IsDefinedAsync(entityType))
            {
                throw new EntityNotTaggableException(entityType);
            }

            var entityTag = new EntityTag(tagId, entityId, tenantId);

            return await _entityTagRepository.InsertAsync(entityTag, cancellationToken: cancellationToken);
        }

        public async Task RemoveTagFromEntityAsync(
            [NotNull] Guid tagId,
            [NotNull] string entityType,
            [NotNull] string entityId,
            [CanBeNull] Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var entityTag = await _entityTagRepository.FindAsync(tagId, entityId, tenantId, cancellationToken);

            await _entityTagRepository.DeleteAsync(entityTag);
        }
    }
}
