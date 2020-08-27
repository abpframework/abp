using System;
using System.Security.Principal;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.MultiTenancy
{
    public class CurrentImpersonatorTenant : ICurrentImpersonatorTenant, ITransientDependency
    {
        public virtual Guid? Id => _principalAccessor.Principal?.FindImpersonatorTenantId();

        private readonly ICurrentPrincipalAccessor _principalAccessor;

        public CurrentImpersonatorTenant(ICurrentPrincipalAccessor principalAccessor)
        {
            _principalAccessor = principalAccessor;
        }
    }
}
