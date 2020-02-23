using Quartz.Impl;
using Quartz.Spi;
using Volo.Abp.Quartz;
using Volo.Abp.Threading;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpQuartzServiceCollectionExtensions
    {
        public static IServiceCollection AddQuartz(this IServiceCollection services, AbpQuartzPreOptions options)
        {
            services.AddSingleton(AsyncHelper.RunSync(() => new StdSchedulerFactory(options.Properties).GetScheduler()));
            return services.AddSingleton(typeof(IJobFactory), typeof(AbpQuartzJobFactory));
        }
    }
}
