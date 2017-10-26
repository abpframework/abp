using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public class NullCurrentTenant : ICurrentTenant, ISingletonDependency
    {
        public static NullCurrentTenant Instance { get; } = new NullCurrentTenant();

        public Guid? Id { get; } = null;
    }
}