using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.ObjectMapping
{
    public static class TestContextRegistrar
    {
        public static IServiceCollection AddTest1AutoObjectMappingProvider<TContext>(this IServiceCollection services)
        {
            return services
                .AddTransient<
                    IAutoObjectMappingProvider<TContext>,
                    Test1AutoObjectMappingProvider<TContext>
                >();
        }

        public static IServiceCollection AddTest2AutoObjectMappingProvider<TContext>(this IServiceCollection services)
        {
            return services
                .AddTransient<
                    IAutoObjectMappingProvider<TContext>,
                    Test2AutoObjectMappingProvider<TContext>
                >();
        }
    }
}