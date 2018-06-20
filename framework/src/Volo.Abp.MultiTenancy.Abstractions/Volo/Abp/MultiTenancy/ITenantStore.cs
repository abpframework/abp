using System;
using System.Threading.Tasks;

namespace Volo.Abp.MultiTenancy
{
    public interface ITenantStore
    {
        Task<TenantInfo> FindAsync(string name);

        Task<TenantInfo> FindAsync(Guid id);
    }
}