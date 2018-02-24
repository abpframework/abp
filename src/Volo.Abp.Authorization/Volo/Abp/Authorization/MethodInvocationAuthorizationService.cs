using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization
{
    public class MethodInvocationAuthorizationService : IMethodInvocationAuthorizationService, ITransientDependency
    {
        private readonly IAuthorizationService _authorizationService;

        public MethodInvocationAuthorizationService(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task CheckAsync(MethodInvocationAuthorizationContext context)
        {
            //TODO: Fully implement! (allow anonymous... etc.)

            var authorizationAttributes = GetAuthorizationDataAttributes(context);
            foreach (var authorizationAttribute in authorizationAttributes)
            {
                await CheckAsync(authorizationAttribute);
            }
        }

        protected virtual IAuthorizeData[] GetAuthorizationDataAttributes(MethodInvocationAuthorizationContext context)
        {
            return context.Method
                .GetCustomAttributes(true)
                .OfType<IAuthorizeData>()
                .ToArray();
        }

        protected async Task CheckAsync(IAuthorizeData authorizationAttribute)
        {
            await _authorizationService.CheckAsync(authorizationAttribute.Policy);
            //TODO: What about roles and other props?
        }
    }
}