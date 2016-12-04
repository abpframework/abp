using Volo.Abp;
using Volo.Abp.Modularity;

namespace Microsoft.Extensions.DependencyInjection
{
    //TODO: Decide to move ABP?
    public static class AbpServiceCollectionExtensions
    {
        public static void AddAbpApplication<TStartupModule>(this IServiceCollection services) //TODO: Simply rename to AddApplication?
            where TStartupModule : IAbpModule
        {
            AbpApplication.Create<TStartupModule>(services);
        }
    }
}