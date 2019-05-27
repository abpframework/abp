using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Validation
{
    public static class ValidationInterceptorRegistrar
    {
        public static void RegisterIfNeeded(IOnServiceRegisteredContext context)
        {
            if (typeof(IValidationEnabled).IsAssignableFrom(context.ImplementationType))
            {
                context.Interceptors.TryAdd<ValidationInterceptor>();
            }
        }
    }
}