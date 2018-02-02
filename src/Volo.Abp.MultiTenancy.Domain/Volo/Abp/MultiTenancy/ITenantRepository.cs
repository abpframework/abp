using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.MultiTenancy
{
    public interface ITenantRepository : IBasicRepository<Tenant, Guid>
    {
        Task<Tenant> FindByNameAsync(string name, CancellationToken cancellationToken = default);

        Task<Tenant> FindByNameIncludeDetailsAsync(string name, CancellationToken cancellationToken = default);

        Task<Tenant> FindWithIncludeDetailsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(CancellationToken cancellationToken = default);

        Task<List<Tenant>> GetListAsync(string sorting, int maxResultCount, int skipCount, string filter, CancellationToken cancellationToken = default);
    }
}