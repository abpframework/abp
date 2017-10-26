using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public class NullCurrentTenant : ICurrentTenant, ISingletonDependency
    {
        public Guid? Id { get; } = null;
    }
}