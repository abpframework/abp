using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Threading;

namespace Microsoft.AspNetCore.Authorization
{
    // TODO: Complete all Sync extension methods!
    public static class AbpAuthorizationServiceExtensions
    {
        public static AuthorizationResult Authorize(this IAuthorizationService authorizationService, ClaimsPrincipal user, object resource, string policyName)
        {
            return AsyncHelper.RunSync(() => authorizationService.AuthorizeAsync(user, resource, policyName));
        }

        public static AuthorizationResult Authorize(this IAuthorizationService authorizationService, ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            return AsyncHelper.RunSync(() => authorizationService.AuthorizeAsync(user, resource, requirements));
        }

        public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, string policyName)
        {
            return AuthorizeAsync(
                authorizationService,
                authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
                policyName
            );
        }

        public static AuthorizationResult Authorize(this IAuthorizationService authorizationService, string policyName)
        {
            return Authorize(
                authorizationService, 
                authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
                policyName
            );
        }

        public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object resource, IAuthorizationRequirement requirement)
        {
            return authorizationService.AuthorizeAsync(
                authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
                resource,
                requirement
            );
        }

        public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object resource, AuthorizationPolicy policy)
        {
            return authorizationService.AuthorizeAsync(
                authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
                resource,
                policy
            );
        }

        public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, AuthorizationPolicy policy)
        {
            return AuthorizeAsync(authorizationService, authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
                policy
            );
        }

        public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            return authorizationService.AuthorizeAsync(
                authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
                resource,
                requirements
            );
        }

        public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object resource, string policyName)
        {
            return authorizationService.AuthorizeAsync(
                authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
                resource,
                policyName
            );
        }

        public static AuthorizationResult Authorize(this IAuthorizationService authorizationService, object resource, string policyName)
        {
            return authorizationService.Authorize(
                authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
                resource,
                policyName
            );
        }

        public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, string policyName)
        {
            return (await authorizationService.AuthorizeAsync(policyName)).Succeeded;
        }

        public static bool IsGranted(this IAuthorizationService authorizationService, string policyName)
        {
            return authorizationService.Authorize(policyName).Succeeded;
        }

        public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, object resource, IAuthorizationRequirement requirement)
        {
            return (await authorizationService.AuthorizeAsync(resource, requirement)).Succeeded;
        }

        public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, object resource, AuthorizationPolicy policy)
        {
            return (await authorizationService.AuthorizeAsync(resource, policy)).Succeeded;
        }

        public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, AuthorizationPolicy policy)
        {
            return (await authorizationService.AuthorizeAsync(policy)).Succeeded;
        }

        public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            return (await authorizationService.AuthorizeAsync(resource, requirements)).Succeeded;
        }

        public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, object resource, string policyName)
        {
            return (await authorizationService.AuthorizeAsync(resource, policyName)).Succeeded;
        }

        public static async Task CheckAsync(this IAuthorizationService authorizationService, string policyName)
        {
            if (!await authorizationService.IsGrantedAsync(policyName))
            {
                throw new AbpAuthorizationException("Authorization failed! Given policy has not granted: " + policyName);
            }
        }

        public static void Check(this IAuthorizationService authorizationService, string policyName)
        {
            AsyncHelper.RunSync(() => authorizationService.CheckAsync(policyName));
        }

        public static async Task CheckAsync(this IAuthorizationService authorizationService, object resource, IAuthorizationRequirement requirement)
        {
            if (!await authorizationService.IsGrantedAsync(resource, requirement))
            {
                throw new AbpAuthorizationException("Authorization failed! Given requirement has not granted for given resource: " + resource);
            }
        }

        public static async Task CheckAsync(this IAuthorizationService authorizationService, object resource, AuthorizationPolicy policy)
        {
            if (!await authorizationService.IsGrantedAsync(resource, policy))
            {
                throw new AbpAuthorizationException("Authorization failed! Given policy has not granted for given resource: " + resource);
            }
        }

        public static async Task CheckAsync(this IAuthorizationService authorizationService, AuthorizationPolicy policy)
        {
            if (!await authorizationService.IsGrantedAsync(policy))
            {
                throw new AbpAuthorizationException("Authorization failed! Given policy has not granted.");
            }
        }

        public static async Task CheckAsync(this IAuthorizationService authorizationService, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            if (!await authorizationService.IsGrantedAsync(resource, requirements))
            {
                throw new AbpAuthorizationException("Authorization failed! Given requirements have not granted for given resource: " + resource);
            }
        }

        public static async Task CheckAsync(this IAuthorizationService authorizationService, object resource, string policyName)
        {
            if (!await authorizationService.IsGrantedAsync(resource, policyName))
            {
                throw new AbpAuthorizationException("Authorization failed! Given polist has not granted for given resource: " + resource);
            }
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