using JetBrains.Annotations;
using Nito.Disposables;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Tags
{
    public class EfCoreEntityTagRepository: EfCoreRepository<ICmsKitDbContext, EntityTag>, IEntityTagRepository
    {
        public EfCoreEntityTagRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task DeleteManyAsync(Guid[] tagIds, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();

            dbSet.RemoveRange(dbSet.Where(x => tagIds.Contains(x.TagId)));
        }

        public Task<EntityTag> FindAsync(
            [NotNull] Guid tagId,
            [NotNull] string entityId,
            [CanBeNull] Guid? tenantId,
            CancellationToken cancellationToken = default)
        {
            return base.FindAsync(x =>
                        x.TagId == tagId &&
                        x.EntityId == entityId &&
                        x.TenantId == tenantId,
                    cancellationToken: cancellationToken);
        }
    }
}