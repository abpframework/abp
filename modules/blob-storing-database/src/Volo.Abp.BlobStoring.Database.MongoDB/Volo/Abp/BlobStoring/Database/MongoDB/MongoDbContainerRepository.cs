using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.BlobStoring.Database.MongoDB
{
    public class MongoDbContainerRepository : MongoDbRepository<IBlobStoringDatabaseMongoDbContext, Container, Guid>, IContainerRepository
    {
        public MongoDbContainerRepository(IMongoDbContextProvider<IBlobStoringDatabaseMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<Container> CreateIfNotExistAsync(string name, Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            var container = await FindAsync(name, tenantId, cancellationToken);

            if (container != null)
            {
                return container;
            }

            container = new Container(Guid.NewGuid(), name, tenantId);
            await InsertAsync(container, true, GetCancellationToken(cancellationToken));

            return container;
        }

        public virtual async Task<Container> FindAsync(string name, Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            return await base.FindAsync(x => x.Name == name, cancellationToken: GetCancellationToken(cancellationToken));
        }
    }
}