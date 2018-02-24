using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization
{
    public class AbpAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider, ITransientDependency
    {
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;

        public AbpAuthorizationPolicyProvider(
            IOptions<AuthorizationOptions> options,
            IPermissionDefinitionManager permissionDefinitionManager) 
            : base(options)
        {
            _permissionDefinitionManager = permissionDefinitionManager;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var permission = _permissionDefinitionManager.GetOrNull(policyName);

            if (permission == null)
            {
                return await base.GetPolicyAsync(policyName);
            }

            //TODO: Optimize!
            var policyBuilder = new AuthorizationPolicyBuilder(Array.Empty<string>());
            policyBuilder.Requirements.Add(new PermissionRequirement(policyName));
            return policyBuilder.Build();
        }
    }
}
