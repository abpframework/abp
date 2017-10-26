using System;

namespace Volo.Abp.Domain.Entities
{
    public interface IMultiTenant
    {
        Guid? TenantId { get; }
    }
}
