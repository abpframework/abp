using JetBrains.Annotations;
using MongoDB.Driver.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.MongoDB.Contents
{
    public class MongoContentRepository : MongoDbRepository<ICmsKitMongoDbContext, Content, Guid>, IContentRepository
    {
        public MongoContentRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(
            dbContextProvider)
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

        public virtual Task DeleteAsync(string entityType, string entityId, Guid? tenantId = null,
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

        public virtual async Task<bool> ExistsAsync([NotNull] string entityType, [NotNull] string entityId,
            Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            Check.NotNullOrEmpty(entityType, nameof(entityType));
            Check.NotNullOrEmpty(entityId, nameof(entityId));

            return await (await GetMongoQueryableAsync(cancellationToken)).AnyAsync(x =>
                    x.EntityType == entityType &&
                    x.EntityId == entityId &&
                    x.TenantId == tenantId,
                cancellationToken: GetCancellationToken(cancellationToken));
        }
    }
}