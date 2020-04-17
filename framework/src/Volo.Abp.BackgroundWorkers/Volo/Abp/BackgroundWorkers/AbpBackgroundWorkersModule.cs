﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers
{
    [DependsOn(
        typeof(AbpThreadingModule)
        )]
    public class AbpBackgroundWorkersModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundWorkerOptions>>().Value;
            if (options.IsEnabled)
            {
                AsyncHelper.RunSync(
                    () => context.ServiceProvider
                        .GetRequiredService<IBackgroundWorkerManager>()
                        .StartAsync()
                );
            }
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundWorkerOptions>>().Value;
            if (options.IsEnabled)
            {
                AsyncHelper.RunSync(
                    () => context.ServiceProvider
                        .GetRequiredService<IBackgroundWorkerManager>()
                        .StopAsync()
                );
            }
        }
    }
}
