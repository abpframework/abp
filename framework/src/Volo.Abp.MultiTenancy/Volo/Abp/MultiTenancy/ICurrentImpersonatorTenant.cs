using System;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public interface ICurrentImpersonatorTenant
    {
        [CanBeNull]
        Guid? Id { get; }
    }
}
