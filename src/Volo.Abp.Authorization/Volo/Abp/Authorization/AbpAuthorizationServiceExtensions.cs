using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Volo.Abp.Authorization
{
    public static class AbpAuthorizationServiceExtensions
    {
        public static Task CheckAsync(this IAuthorizationService authorizationService, string policyName)
        {
            return authorizationService.AsAbpAuthorizationService().CheckAsync(policyName);
        }

        private static IAbpAuthorizationService AsAbpAuthorizationService(this IAuthorizationService authorizationService)
        {
            if (!(authorizationService is IAbpAuthorizationService abpAuthorizationService))
            {
                throw new AbpException($"{nameof(authorizationService)} should implement {typeof(IAbpAuthorizationService).FullName}");
            }

            return abpAuthorizationService;
        }
    }
}