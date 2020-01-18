using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Clients;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Volo.Abp.Authorization
{
    public class MethodInvocationAuthorizationService : IMethodInvocationAuthorizationService, ITransientDependency
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentClient _currentClient;
        private readonly IAbpAuthorizationPolicyProvider _abpAuthorizationPolicyProvider;

        public MethodInvocationAuthorizationService(
            IAuthorizationService authorizationService, 
            ICurrentUser currentUser,
            ICurrentClient currentClient,
            IAbpAuthorizationPolicyProvider abpAuthorizationPolicyProvider)
        {
            _authorizationService = authorizationService;
            _currentUser = currentUser;
            _currentClient = currentClient;
            _abpAuthorizationPolicyProvider = abpAuthorizationPolicyProvider;
        }

        public async Task CheckAsync(MethodInvocationAuthorizationContext context)
        {
            if (AllowAnonymous(context))
            {
                return;
            }

            foreach (var authorizationAttribute in GetAuthorizationDataAttributes(context.Method))
            {
                await CheckAsync(authorizationAttribute).ConfigureAwait(false);
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

            if (methodInfo.IsPublic)
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

        protected async Task CheckAsync(IAuthorizeData authorizationAttribute)
        {
            var authorizationPolicy = await AuthorizationPolicy.CombineAsync(
                                                    _abpAuthorizationPolicyProvider,
                                                    new List<IAuthorizeData> { authorizationAttribute });
            await _authorizationService.CheckAsync(authorizationPolicy).ConfigureAwait(false);
        }
    }
}