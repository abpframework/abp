using Volo.Abp.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionLoggingExtensions
    {
        public static IInitLogger GetInitLogger(this IServiceCollection services)
        {
            return services.GetSingletonInstance<IInitLogger>();
        }
    }
}