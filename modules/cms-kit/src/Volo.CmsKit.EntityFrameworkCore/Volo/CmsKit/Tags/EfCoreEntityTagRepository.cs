using JetBrains.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Tags
{
    public class EfCoreEntityTagRepository : EfCoreRepository<ICmsKitDbContext, EntityTag>, IEntityTagRepository
    {
        public EfCoreEntityTagRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(
            dbContextProvider)
        {
        }

        public virtual async Task DeleteManyAsync(Guid[] tagIds, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var dbSet = dbContext.Set<EntityTag>();
            dbSet.RemoveRange(dbSet.Where(x => tagIds.Contains(x.TagId)));
            await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
        }

        public virtual Task<EntityTag> FindAsync(
            [NotNull] Guid tagId,
            [NotNull] string entityId,
            [CanBeNull] Guid? tenantId,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrEmpty(entityId, nameof(entityId));
            
            return base.FindAsync(x =>
                    x.TagId == tagId &&
                    x.EntityId == entityId &&
                    x.TenantId == tenantId,
                cancellationToken: GetCancellationToken(cancellationToken));
        }
    }
}