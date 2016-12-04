using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore
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