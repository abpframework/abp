using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    [Dependency(ReplaceServices = true)]
    public class MultiTenancyManagerCurrentTenant : ICurrentTenant, ITransientDependency
    {
        public Guid? Id => _multiTenancyManager.CurrentTenant?.Id;

        private readonly IMultiTenancyManager _multiTenancyManager;

        public MultiTenancyManagerCurrentTenant(IMultiTenancyManager multiTenancyManager)
        {
            _multiTenancyManager = multiTenancyManager;
        }
    }
}