using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy.EntityFrameworkCore;

namespace Volo.Abp.MultiTenancy
{
    public class EfCoreTenantRepository : EfCoreRepository<IMultiTenancyDbContext, Tenant, Guid>, ITenantRepository
    {
        public EfCoreTenantRepository(IDbContextProvider<IMultiTenancyDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<Tenant> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .FirstOrDefaultAsync(t => t.Name == name, cancellationToken);
        }

        public async Task<Tenant> FindByNameIncludeDetailsAsync(string name, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(t => t.ConnectionStrings) //TODO: Why not creating a virtual Include method in EfCoreRepository and override to add included properties to be available for every query..?
                .FirstOrDefaultAsync(t => t.Name == name, cancellationToken);
        }

        public async Task<Tenant> FindWithIncludeDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(t => t.ConnectionStrings)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.LongCountAsync(cancellationToken);
        }

        public async Task<List<Tenant>> GetListAsync(string sorting, int maxResultCount, int skipCount, string filter, CancellationToken cancellationToken = default)
        {
            return await this.WhereIf(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Name.Contains(filter)
                )
                .OrderBy(sorting ?? nameof(Tenant.Name))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(cancellationToken);
        }
    }
}
