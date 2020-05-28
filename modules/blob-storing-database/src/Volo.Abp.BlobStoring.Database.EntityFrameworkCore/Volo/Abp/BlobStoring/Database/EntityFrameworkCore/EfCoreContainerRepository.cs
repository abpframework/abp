using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.BlobStoring.Database.EntityFrameworkCore
{
    public class EfCoreContainerRepository : EfCoreRepository<IBlobStoringDatabaseDbContext, Container, Guid>, IContainerRepository
    {
        public EfCoreContainerRepository(IDbContextProvider<IBlobStoringDatabaseDbContext> dbContextProvider) : base(dbContextProvider)
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
            await base.InsertAsync(container, true, GetCancellationToken(cancellationToken));

            return container;
        }
        
        public virtual async Task<Container> FindAsync(string name, Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            return await DbSet.WhereIf(tenantId != null, x => x.TenantId == tenantId)
                        .FirstOrDefaultAsync(x => x.Name == name, GetCancellationToken(cancellationToken));
        }
    }
}