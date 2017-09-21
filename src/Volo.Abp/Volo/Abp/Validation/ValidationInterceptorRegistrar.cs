using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Validation
{
    public static class ValidationInterceptorRegistrar
    {
        public static void RegisterIfNeeded(IOnServiceRegistredContext context)
        {
            if (typeof(IApplicationService).IsAssignableFrom(context.ImplementationType))
            {
                //TODO: Notice that it may add the interceptor more than one for every exposed service type!?
                context.Interceptors.Add<ValidationInterceptor>();
            }
        }
    }
}