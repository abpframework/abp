using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityService.Host.Controllers
{
    [ApiController]
    [Route("api/identity/abpApplication")]
    public class AbpApplicationController : AbpController
    {
        private readonly IAbpAuthorizationPolicyProvider _abpAuthorizationPolicyProvider;
        private readonly IAbpAuthorizationService _authorizationService;
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;
        public AbpApplicationController(IAbpAuthorizationPolicyProvider abpAuthorizationPolicyProvider, IAbpAuthorizationService authorizationService, IPermissionDefinitionManager permissionDefinitionManager)
        {
            _abpAuthorizationPolicyProvider = abpAuthorizationPolicyProvider;
            _authorizationService = authorizationService;
            _permissionDefinitionManager = permissionDefinitionManager;
        }

        [HttpGet("getAuthConfig")]
        public async Task<ApplicationAuthConfigurationDto> GetAuthConfigAsync()
        {
            var authConfig = new ApplicationAuthConfigurationDto();


            foreach (var policyName in await _abpAuthorizationPolicyProvider.GetPoliciesNamesAsync())
            {
                authConfig.Policies[policyName] = true;

                if (await _authorizationService.IsGrantedAsync(policyName))
                {
                    authConfig.GrantedPolicies[policyName] = true;
                }
            }

            foreach (var group in _permissionDefinitionManager.GetGroups())
            {
                authConfig.Policies[group.Name] = true;


                authConfig.GrantedPolicies[group.Name] = true;
            }

            return authConfig;
        }
    }
}
