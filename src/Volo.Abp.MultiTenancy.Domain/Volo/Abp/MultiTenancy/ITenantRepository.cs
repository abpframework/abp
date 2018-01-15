using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.MultiTenancy
{
    public interface ITenantRepository : IRepository<Tenant, Guid>
    {
        Task<Tenant> FindByNameIncludeDetailsAsync(string name);

        Task<Tenant> FindWithIncludeDetailsAsync(Guid id);
    }
}