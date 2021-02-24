using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Volo.CmsKit.Tags
{
    public class EntityTagManager : DomainService
    {
        protected IEntityTagRepository EntityTagRepository { get; }
        protected ITagRepository TagRepository { get; }
        protected ITagDefinitionStore TagDefinitionStore { get; }
        protected TagManager TagManager { get; }

        public EntityTagManager(
            IEntityTagRepository entityTagRepository,
            ITagRepository tagRepository,
            ITagDefinitionStore tagDefinitionStore,
            TagManager tagManager)
        {
            EntityTagRepository = entityTagRepository;
            TagRepository = tagRepository;
            TagDefinitionStore = tagDefinitionStore;
            TagManager = tagManager;
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

        public async Task SetEntityTagsAsync(string entityType, string entityId, List<string> tags)
        {
            var existingTags =
              await TagRepository.GetAllRelatedTagsAsync(entityType, entityId);

            var deletedTags = existingTags.Where(x => !tags.Contains(x.Name)).ToList();
            var addedTags = tags.Where(x => !existingTags.Any(a => a.Name == x));

            await EntityTagRepository.DeleteManyAsync(deletedTags.Select(s => s.Id).ToArray());

            foreach (var addedTag in addedTags)
            {
                var tag = await TagManager.GetOrAddAsync(entityType, addedTag);

                await AddTagToEntityAsync(tag.Id, entityType, entityId, CurrentTenant?.Id);
            }
        }
    }
}