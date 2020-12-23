using JetBrains.Annotations;
using MongoDB.Driver.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.MongoDB.Tags
{
    public class MongoTagRepository : MongoDbRepository<ICmsKitMongoDbContext, Tag, Guid>, ITagRepository
    {
        public MongoTagRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public Task<bool> AnyAsync(
            [NotNull] string entityType,
            [NotNull] string name,
            Guid? tenantId,
            CancellationToken cancellationToken = default)
        {
            return GetMongoQueryable()
                    .AnyAsync(x =>
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
