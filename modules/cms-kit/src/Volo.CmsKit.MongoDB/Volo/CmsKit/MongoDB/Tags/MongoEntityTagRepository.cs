using JetBrains.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.MongoDB.Tags
{
    public class MongoEntityTagRepository: MongoDbRepository<ICmsKitMongoDbContext, EntityTag>, IEntityTagRepository
    {
        public MongoEntityTagRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
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