using System.Linq;
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

        public MethodInvocationAuthorizationService(
            IAuthorizationService authorizationService, 
            ICurrentUser currentUser,
            ICurrentClient currentClient)
        {
            _authorizationService = authorizationService;
            _currentUser = currentUser;
            _currentClient = currentClient;
        }

        public async Task CheckAsync(MethodInvocationAuthorizationContext context)
        {
            if (AllowAnonymous(context))
            {
                return;
            }

            var authorizationAttributes = GetAuthorizationDataAttributes(context);
            foreach (var authorizationAttribute in authorizationAttributes)
            {
                await CheckAsync(authorizationAttribute);
            }
        }

        protected virtual bool AllowAnonymous(MethodInvocationAuthorizationContext context)
        {
            return context.Method.GetCustomAttributes(true).OfType<IAllowAnonymous>().Any();
        }

        protected virtual IAuthorizeData[] GetAuthorizationDataAttributes(MethodInvocationAuthorizationContext context)
        {
            var classAttributes = context.Method.DeclaringType
                .GetCustomAttributes(true)
                .OfType<IAuthorizeData>();

            var methodAttributes = context.Method
                .GetCustomAttributes(true)
                .OfType<IAuthorizeData>();

            return classAttributes.Union(methodAttributes).ToArray();
        }

        protected async Task CheckAsync(IAuthorizeData authorizationAttribute)
        {
            if (authorizationAttribute.Policy == null)
            {
                //TODO: Can we find a better, unified, way of checking if current request has been authenticated
                if (!_currentUser.IsAuthenticated && !_currentClient.IsAuthenticated)
                {
                    throw new AbpAuthorizationException("Authorization failed! User has not logged in.");
                }
            }
            else
            {
                await _authorizationService.CheckAsync(authorizationAttribute.Policy);
            }

            //TODO: What about roles and other props?
        }
    }
}