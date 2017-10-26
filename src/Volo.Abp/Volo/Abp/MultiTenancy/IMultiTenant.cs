using System;

namespace Volo.Abp.MultiTenancy
{
    public interface IMultiTenant
    {
        Guid? TenantId { get; }
    }
}
