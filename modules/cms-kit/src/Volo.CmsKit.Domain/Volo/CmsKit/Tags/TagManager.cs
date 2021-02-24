using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Volo.CmsKit.Tags
{
    public class TagManager : DomainService
    {
        protected ITagRepository TagRepository { get; }
        protected ITagDefinitionStore TagDefinitionStore { get; }

        public TagManager(ITagRepository tagRepository, ITagDefinitionStore tagDefinitionStore)
        {
            TagRepository = tagRepository;
            TagDefinitionStore = tagDefinitionStore;
        }

        public virtual async Task<Tag> GetOrAddAsync([NotNull] string entityType, [NotNull] string name)
        {
            return await TagRepository.FindAsync(entityType, name)
                    ?? await InsertAsync(GuidGenerator.Create(), entityType, name);
        }

        public virtual async Task<Tag> InsertAsync(Guid id,
                                                   [NotNull] string entityType,
                                                   [NotNull] string name)
        {
            if (!await TagDefinitionStore.IsDefinedAsync(entityType))
            {
                throw new EntityNotTaggableException(entityType);
            }

            if (await TagRepository.AnyAsync(entityType, name))
            {
                throw new TagAlreadyExistException(entityType, name);
            }

            return await TagRepository.InsertAsync(
                new Tag(id, entityType, name, CurrentTenant.Id));
        }

        public virtual async Task<Tag> UpdateAsync(Guid id,
                                                   [NotNull] string name)
        {
            Check.NotNullOrEmpty(name, nameof(name));

            var entity = await TagRepository.GetAsync(id);

            if (name != entity.Name &&
                await TagRepository.AnyAsync(entity.EntityType, name))
            {
                throw new TagAlreadyExistException(entity.EntityType, name);
            }

            entity.SetName(name);

            return await TagRepository.UpdateAsync(entity);
        }
    }
}