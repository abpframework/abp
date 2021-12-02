using System;
using System.Threading.Tasks;

namespace Volo.Abp.MultiTenancy;

public interface ITenantStore
{
    Task<TenantConfiguration> FindAsync(string name);

    Task<TenantConfiguration> FindAsync(Guid id);

    [Obsolete("Use FindAsync method.")]
    TenantConfiguration Find(string name);

    [Obsolete("Use FindAsync method.")]
    TenantConfiguration Find(Guid id);
}
