using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization
{
    public static class AuthorizationInterceptorRegistrar
    {
        public static void RegisterIfNeeded(IOnServiceRegistredContext context)
        {
            if (typeof(IAuthorizationEnabled).IsAssignableFrom(context.ImplementationType))
            {
                context.Interceptors.TryAdd<AuthorizationInterceptor>();
            }
        }
    }
}