using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Volo.CmsKit.Tags
{
    public class TagManager : DomainService, ITagManager
    {
        protected ITagRepository TagRepository { get; }
        protected ITagDefinitionStore TagDefinitionStore { get; }

        public TagManager(ITagRepository tagRepository, ITagDefinitionStore tagDefinitionStore)
        {
            TagRepository = tagRepository;
            TagDefinitionStore = tagDefinitionStore;
        }

        public virtual async Task<Tag> GetOrAddAsync([NotNull] string entityType, [NotNull] string name,
            Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            var entity = await TagRepository.FindAsync(entityType, name, tenantId, cancellationToken);

            if (entity == null)
            {
                entity = await InsertAsync(GuidGenerator.Create(), entityType, name, tenantId, cancellationToken);
            }

            return entity;
        }

        public virtual async Task<Tag> InsertAsync(Guid id, [NotNull] string entityType, [NotNull] string name,
            Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            if (!await TagDefinitionStore.IsDefinedAsync(entityType))
            {
                throw new EntityNotTaggableException(entityType);
            }

            if (await TagRepository.AnyAsync(entityType, name, tenantId, cancellationToken))
            {
                throw new TagAlreadyExistException(entityType, name);
            }

            return await TagRepository.InsertAsync(new Tag(id, entityType, name, tenantId),
                cancellationToken: cancellationToken);
        }

        public virtual async Task<Tag> UpdateAsync(Guid id, [NotNull] string name,
            CancellationToken cancellationToken = default)
        {
            var entity = await TagRepository.GetAsync(id, cancellationToken: cancellationToken);

            if (name != entity.Name &&
                await TagRepository.AnyAsync(entity.EntityType, name, entity.TenantId, cancellationToken))
            {
                throw new TagAlreadyExistException(entity.EntityType, name);
            }

            entity.SetName(name);

            return await TagRepository.UpdateAsync(entity, cancellationToken: cancellationToken);
        }

        public virtual Task<List<TagEntityTypeDefiniton>> GetTagDefinitionsAsync(
            CancellationToken cancellationToken = default)
        {
            return TagDefinitionStore.GetTagEntityTypeDefinitionListAsync();
        }
    }
}