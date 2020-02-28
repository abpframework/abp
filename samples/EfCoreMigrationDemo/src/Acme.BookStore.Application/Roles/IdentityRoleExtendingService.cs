using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace Acme.BookStore.Roles
{
    public class IdentityRoleExtendingService : ITransientDependency
    {
        private readonly IIdentityRoleRepository _identityRoleRepository;

        public IdentityRoleExtendingService(IIdentityRoleRepository identityRoleRepository)
        {
            _identityRoleRepository = identityRoleRepository;
        }

        
    }
}
