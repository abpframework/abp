using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization
{
    public class MethodInvocationAuthorizationService : IMethodInvocationAuthorizationService, ITransientDependency
    {
        private static readonly char[] _separator = new[] { ',' };

        private readonly IAbpAuthorizationPolicyProvider _abpAuthorizationPolicyProvider;
        private readonly IAbpAuthorizationService _abpAuthorizationService;

        public MethodInvocationAuthorizationService(
            IAbpAuthorizationPolicyProvider abpAuthorizationPolicyProvider,
            IAbpAuthorizationService abpAuthorizationService)
        {
            _abpAuthorizationPolicyProvider = abpAuthorizationPolicyProvider;
            _abpAuthorizationService = abpAuthorizationService;
        }

        public async Task CheckAsync(MethodInvocationAuthorizationContext context)
        {
            if (AllowAnonymous(context))
            {
                return;
            }

            var authorizationAttributes = GetAuthorizationDataAttributes(context.Method);

            if (authorizationAttributes.Where(v => !string.IsNullOrWhiteSpace(v.Policy)).Any(v => v.Policy.Contains(_separator[0])))
            {
                var policyNames = authorizationAttributes.Where(v => !string.IsNullOrWhiteSpace(v.Policy))
                    .SelectMany(v => v.Policy.Split(_separator, StringSplitOptions.RemoveEmptyEntries))
                    .ToArray();
                var isGranted = await _abpAuthorizationService.IsGrantedAnyAsync(policyNames);

                if (!isGranted)
                {
                    throw new AbpAuthorizationException(code: AbpAuthorizationErrorCodes.GivenPolicyHasNotGranted);
                }
            }
            else
            {
                var authorizationPolicy = await AuthorizationPolicy.CombineAsync(
                    _abpAuthorizationPolicyProvider,
                    GetAuthorizationDataAttributes(context.Method)
                );

                if (authorizationPolicy == null)
                {
                    return;
                }

                await _abpAuthorizationService.CheckAsync(authorizationPolicy);
            }
        }

        protected virtual bool AllowAnonymous(MethodInvocationAuthorizationContext context)
        {
            return context.Method.GetCustomAttributes(true).OfType<IAllowAnonymous>().Any();
        }

        protected virtual IEnumerable<IAuthorizeData> GetAuthorizationDataAttributes(MethodInfo methodInfo)
        {
            var attributes = methodInfo
                .GetCustomAttributes(true)
                .OfType<IAuthorizeData>();

            if (methodInfo.IsPublic && methodInfo.DeclaringType != null)
            {
                attributes = attributes
                    .Union(
                        methodInfo.DeclaringType
                            .GetCustomAttributes(true)
                            .OfType<IAuthorizeData>()
                    );
            }

            return attributes;
        }
    }
}
