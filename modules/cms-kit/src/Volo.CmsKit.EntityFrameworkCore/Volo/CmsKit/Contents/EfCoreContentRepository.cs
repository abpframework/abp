using System;
using System.Threading;
using System.Threading.Tasks;
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

        public Task<Content> GetAsync(
            string entityType,
            string entityId,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            return GetAsync(x =>
                    !x.IsDeleted &&
                    x.EntityType == entityType &&
                    x.EntityId == entityId &&
                    x.TenantId == tenantId,
                cancellationToken: GetCancellationToken(cancellationToken)
            );
        }
        
        public Task<Content> FindAsync(
            string entityType,
            string entityId,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            return FindAsync(x =>
                    !x.IsDeleted &&
                    x.EntityType == entityType &&
                    x.EntityId == entityId &&
                    x.TenantId == tenantId,
                cancellationToken: GetCancellationToken(cancellationToken)
                );
        }

        public Task DeleteAsync(string entityType, string entityId, Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            return DeleteAsync(x =>
                        x.EntityType == entityType &&
                        x.EntityId == entityId &&
                        x.TenantId == tenantId,
                        cancellationToken: GetCancellationToken(cancellationToken));
        }
    }
}