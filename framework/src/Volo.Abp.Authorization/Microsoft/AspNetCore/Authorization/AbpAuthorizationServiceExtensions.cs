﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Authorization;

namespace Microsoft.AspNetCore.Authorization
{
    public static class AbpAuthorizationServiceExtensions
    {
        public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, string policyName)
        {
            return await AuthorizeAsync(
                authorizationService,
                null,
                policyName
            ).ConfigureAwait(false);
        }

        public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object resource, IAuthorizationRequirement requirement)
        {
            return await authorizationService.AuthorizeAsync(
                authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
                resource,
                requirement
            ).ConfigureAwait(false);
        }

        public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object resource, AuthorizationPolicy policy)
        {
            return await authorizationService.AuthorizeAsync(
                authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
                resource,
                policy
            ).ConfigureAwait(false);
        }

        public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, AuthorizationPolicy policy)
        {
            return await AuthorizeAsync(
                authorizationService,
                null,
                policy
            ).ConfigureAwait(false);
        }

        public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            return await authorizationService.AuthorizeAsync(
                authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
                resource,
                requirements
            ).ConfigureAwait(false);
        }

        public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object resource, string policyName)
        {
            return await authorizationService.AuthorizeAsync(
                authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
                resource,
                policyName
            ).ConfigureAwait(false);
        }

        public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, string policyName)
        {
            return (await authorizationService.AuthorizeAsync(policyName).ConfigureAwait(false)).Succeeded;
        }

        public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, object resource, IAuthorizationRequirement requirement)
        {
            return (await authorizationService.AuthorizeAsync(resource, requirement).ConfigureAwait(false)).Succeeded;
        }

        public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, object resource, AuthorizationPolicy policy)
        {
            return (await authorizationService.AuthorizeAsync(resource, policy).ConfigureAwait(false)).Succeeded;
        }

        public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, AuthorizationPolicy policy)
        {
            return (await authorizationService.AuthorizeAsync(policy).ConfigureAwait(false)).Succeeded;
        }

        public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            return (await authorizationService.AuthorizeAsync(resource, requirements).ConfigureAwait(false)).Succeeded;
        }

        public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, object resource, string policyName)
        {
            return (await authorizationService.AuthorizeAsync(resource, policyName).ConfigureAwait(false)).Succeeded;
        }

        public static async Task CheckAsync(this IAuthorizationService authorizationService, string policyName)
        {
            if (!await authorizationService.IsGrantedAsync(policyName).ConfigureAwait(false))
            {
                throw new AbpAuthorizationException("Authorization failed! Given policy has not granted: " + policyName);
            }
        }

        public static async Task CheckAsync(this IAuthorizationService authorizationService, object resource, IAuthorizationRequirement requirement)
        {
            if (!await authorizationService.IsGrantedAsync(resource, requirement).ConfigureAwait(false))
            {
                throw new AbpAuthorizationException("Authorization failed! Given requirement has not granted for given resource: " + resource);
            }
        }

        public static async Task CheckAsync(this IAuthorizationService authorizationService, object resource, AuthorizationPolicy policy)
        {
            if (!await authorizationService.IsGrantedAsync(resource, policy).ConfigureAwait(false))
            {
                throw new AbpAuthorizationException("Authorization failed! Given policy has not granted for given resource: " + resource);
            }
        }

        public static async Task CheckAsync(this IAuthorizationService authorizationService, AuthorizationPolicy policy)
        {
            if (!await authorizationService.IsGrantedAsync(policy).ConfigureAwait(false))
            {
                throw new AbpAuthorizationException("Authorization failed! Given policy has not granted.");
            }
        }

        public static async Task CheckAsync(this IAuthorizationService authorizationService, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            if (!await authorizationService.IsGrantedAsync(resource, requirements).ConfigureAwait(false))
            {
                throw new AbpAuthorizationException("Authorization failed! Given requirements have not granted for given resource: " + resource);
            }
        }

        public static async Task CheckAsync(this IAuthorizationService authorizationService, object resource, string policyName)
        {
            if (!await authorizationService.IsGrantedAsync(resource, policyName).ConfigureAwait(false))
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