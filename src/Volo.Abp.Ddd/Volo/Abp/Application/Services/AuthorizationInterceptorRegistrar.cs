using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Application.Services
{
    public static class AuthorizationInterceptorRegistrar
    {
        public static void RegisterIfNeeded(IOnServiceRegistredContext context)
        {
            if (typeof(IApplicationService).IsAssignableFrom(context.ImplementationType))
            {
                context.Interceptors.TryAdd<AuthorizationInterceptor>();
            }
        }
    }
}