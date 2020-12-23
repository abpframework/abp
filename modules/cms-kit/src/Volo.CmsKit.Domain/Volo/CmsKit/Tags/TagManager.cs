using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            [CanBeNull] string hexColor = null,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            var entity = await _tagRepository.FindAsync(entityType, name, tenantId, cancellationToken);

            if (entity == null)
            {
                entity = await InsertAsync(entityType, name, hexColor, tenantId, cancellationToken);
            }

            return entity;
        }

        public async Task<Tag> InsertAsync(
            [NotNull] string entityType,
            [NotNull] string name,
            [CanBeNull] string hexColor,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            if (await _tagRepository.AnyAsync(entityType, name, tenantId, cancellationToken))
            {
                throw new BusinessException(message: "Tag already exist!"); // Already Exist
            }

            return await _tagRepository.InsertAsync(
                            new Tag(
                                entityType,
                                name,
                                hexColor,
                                tenantId),
                            cancellationToken: cancellationToken);
        }

        public async Task<Tag> UpdateAsync(
            Guid id,
            [NotNull] string name,
            [CanBeNull] string hexColor,
            CancellationToken cancellationToken = default)
        {
            var entity = await _tagRepository.GetAsync(id, cancellationToken: cancellationToken);

            entity.SetName(name);
            entity.SetHexColor(hexColor);

            if (await _tagRepository.AnyAsync(entity.EntityType, name, entity.TenantId, cancellationToken))
            {
                throw new BusinessException(message: "Tag already exist!"); // Already Exist
            }

            return await _tagRepository.UpdateAsync(entity);
        }
    }
}
