using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Tags
{
    public class EfCoreTagRepository : EfCoreRepository<ICmsKitDbContext, Tag, Guid>, ITagRepository
    {
        public EfCoreTagRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public Task<bool> AnyAsync(
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

        public Task<Tag> GetAsync(
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

        public Task<Tag> FindAsync(
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
    }
}
