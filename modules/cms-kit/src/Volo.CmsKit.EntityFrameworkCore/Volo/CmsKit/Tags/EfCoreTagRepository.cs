using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Tags
{
    public class EfCoreTagRepository : EfCoreRepository<ICmsKitDbContext, Tag, Guid>, ITagRepository
    {
        public EfCoreTagRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual Task<bool> AnyAsync(
            [NotNull] string entityType,
            [NotNull] string name,
            Guid? tenantId,
            CancellationToken cancellationToken = default)
        {
            return DbSet.AnyAsync(x =>
                    x.EntityType == entityType &&
                    x.Name == name &&
                    x.TenantId == tenantId,
                cancellationToken);
        }

        public virtual Task<Tag> GetAsync(
            [NotNull] string entityType,
            [NotNull] string name,
            Guid? tenantId,
            CancellationToken cancellationToken = default)
        {
            return GetAsync(x =>
                    x.EntityType == entityType &&
                    x.Name == name &&
                    x.TenantId == tenantId,
                cancellationToken: cancellationToken);
        }

        public virtual Task<Tag> FindAsync(
            [NotNull] string entityType,
            [NotNull] string name,
            Guid? tenantId,
            CancellationToken cancellationToken = default)
        {
            return FindAsync(x =>
                    x.EntityType == entityType &&
                    x.Name == name &&
                    x.TenantId == tenantId,
                cancellationToken: cancellationToken);
        }

        public virtual async Task<List<Tag>> GetAllRelatedTagsAsync(
            [NotNull] string entityType,
            [NotNull] string entityId,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {

            var query = DbContext.Set<EntityTag>()
                .Where(x =>
                    x.EntityId == entityId &&
                    x.TenantId == tenantId
                    )
                .Join(
                    DbSet,
                    o => o.TagId,
                    i => i.Id,
                    (entityTag, tag) => tag)
                .Where(x => x.EntityType == entityType);

            return await query.ToListAsync();
        }
    }
}
