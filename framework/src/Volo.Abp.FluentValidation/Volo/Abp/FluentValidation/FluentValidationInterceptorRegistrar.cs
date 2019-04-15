using FluentValidation;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.FluentValidation
{
    public static class FluentValidationInterceptorRegistrar
    {
        public static void RegisterIfNeeded(IOnServiceRegistredContext context)
        {
            if (typeof(IValidator).IsAssignableFrom(context.ImplementationType))
            {
                context.Interceptors.TryAdd<FluentValidationInterceptor>();
            }
        }
    }
}
