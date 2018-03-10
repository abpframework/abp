using Volo.Abp.DependencyInjection;
using Volo.Abp.Validation;

namespace Volo.Abp.Application.Services
{
    public static class ValidationInterceptorRegistrar
    {
        public static void RegisterIfNeeded(IOnServiceRegistredContext context)
        {
            //TODO Create IValidationEnabled interface and inherit IApplicationService from it!
            if (typeof(IApplicationService).IsAssignableFrom(context.ImplementationType))
            {
                context.Interceptors.TryAdd<ValidationInterceptor>();
            }
        }
    }
}