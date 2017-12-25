using System;

namespace Volo.Abp.MultiTenancy
{
    public interface ICurrentTenant
    {
        Guid? Id { get; }
    }
}
