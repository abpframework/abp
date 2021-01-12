using JetBrains.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Volo.CmsKit.Tags
{
    public class TagManager : DomainService, ITagManager
    {
        private readonly ITagRepository _tagRepository;

        public TagManager(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<Tag> GetOrAddAsync(
            [NotNull] string entityType,
            [NotNull] string name,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var entity = await _tagRepository.FindAsync(entityType, name, tenantId, cancellationToken);

            if (entity == null)
            {
                entity = await InsertAsync(GuidGenerator.Create(), entityType, name, tenantId, cancellationToken);
            }

            return entity;
        }

        public async Task<Tag> InsertAsync(
            Guid id,
            [NotNull] string entityType,
            [NotNull] string name,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            if (await _tagRepository.AnyAsync(entityType, name, tenantId, cancellationToken))
            {
                throw new TagAlreadyExistException(entityType, name);
            }

            return await _tagRepository.InsertAsync(
                new Tag(
                    id,
                    entityType,
                    name,
                    tenantId),
                cancellationToken: cancellationToken);
        }

        public async Task<Tag> UpdateAsync(
            Guid id,
            [NotNull] string name,
            CancellationToken cancellationToken = default)
        {
            var entity = await _tagRepository.GetAsync(id, cancellationToken: cancellationToken);

            if (name != entity.Name &&
                await _tagRepository.AnyAsync(entity.EntityType, name, entity.TenantId, cancellationToken))
            {
                throw new TagAlreadyExistException(entity.EntityType, name);
            }

            entity.SetName(name);

            return await _tagRepository.UpdateAsync(entity, cancellationToken: cancellationToken);
        }
    }
}