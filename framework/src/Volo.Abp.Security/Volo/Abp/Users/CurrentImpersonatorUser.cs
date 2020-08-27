using System;
using System.Security.Principal;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Users
{
    public class CurrentImpersonatorUser : ICurrentImpersonatorUser, ITransientDependency
    {
        public virtual Guid? Id => _principalAccessor.Principal?.FindImpersonatorUserId();

        private readonly ICurrentPrincipalAccessor _principalAccessor;

        public CurrentImpersonatorUser(ICurrentPrincipalAccessor principalAccessor)
        {
            _principalAccessor = principalAccessor;
        }
    }
}
