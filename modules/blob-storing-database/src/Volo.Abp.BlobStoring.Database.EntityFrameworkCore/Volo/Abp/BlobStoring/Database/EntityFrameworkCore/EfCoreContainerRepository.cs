using Microsoft.EntityFrameworkCore;
using System;
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

        public virtual async Task<Container> GetContainerAsync(string name, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstAsync(x => x.Name == name, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<Container> FindContainerAsync(string name, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.Name == name, GetCancellationToken(cancellationToken));
        }
    }
}