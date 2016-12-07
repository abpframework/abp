using Volo.Abp;
using Volo.Abp.Modularity;

namespace Microsoft.Extensions.DependencyInjection
{
    //TODO: Decide to move ABP?
    public static class AbpServiceCollectionExtensions
    {
        public static AbpApplication AddApplication<TStartupModule>(this IServiceCollection services)
            where TStartupModule : IAbpModule
        {
            return AbpApplication.Create<TStartupModule>(services);
        }
    }
}