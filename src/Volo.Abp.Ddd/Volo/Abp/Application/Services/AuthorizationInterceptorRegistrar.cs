using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Application.Services
{
    public static class AuthorizationInterceptorRegistrar
    {
        public static void RegisterIfNeeded(IOnServiceRegistredContext context)
        {
            //TODO Create IAuthorizationEnabled interface and inherit IApplicationService from it!
            if (typeof(IApplicationService).IsAssignableFrom(context.ImplementationType))
            {
                context.Interceptors.TryAdd<AuthorizationInterceptor>();
            }
        }
    }
}