using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Contents
{
    public class EfCoreContentRepository : EfCoreRepository<ICmsKitDbContext, Content, Guid>, IContentRepository
    {
        public EfCoreContentRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual Task<Content> GetAsync(
            string entityType,
            string entityId,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrEmpty(entityType, nameof(entityType));
            Check.NotNullOrEmpty(entityId, nameof(entityId));
            
            return GetAsync(x =>
                    x.EntityType == entityType &&
                    x.EntityId == entityId &&
                    x.TenantId == tenantId,
                cancellationToken: GetCancellationToken(cancellationToken)
            );
        }
        
        public virtual Task<Content> FindAsync(
            string entityType,
            string entityId,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrEmpty(entityType, nameof(entityType));
            Check.NotNullOrEmpty(entityId, nameof(entityId));
            
            return FindAsync(x =>
                    x.EntityType == entityType &&
                    x.EntityId == entityId &&
                    x.TenantId == tenantId,
                cancellationToken: GetCancellationToken(cancellationToken)
                );
        }

        public virtual Task DeleteAsync(
            string entityType,
            string entityId,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrEmpty(entityType, nameof(entityType));
            Check.NotNullOrEmpty(entityId, nameof(entityId));
            
            return DeleteAsync(x =>
                        x.EntityType == entityType &&
                        x.EntityId == entityId &&
                        x.TenantId == tenantId,
                        cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task<bool> ExistsAsync(
            [NotNull] string entityType,
            [NotNull] string entityId,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrEmpty(entityType, nameof(entityType));
            Check.NotNullOrEmpty(entityId, nameof(entityId));
            
            var dbSet = await GetDbSetAsync();
            return await dbSet.AnyAsync(x =>
                        x.EntityType == entityType &&
                        x.EntityId == entityId &&
                        x.TenantId == tenantId,
                        cancellationToken: GetCancellationToken(cancellationToken));
        }
    }
}