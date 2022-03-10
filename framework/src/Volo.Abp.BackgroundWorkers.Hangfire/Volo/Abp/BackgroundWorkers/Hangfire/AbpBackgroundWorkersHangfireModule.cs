﻿using System;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Hangfire;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers.Hangfire;

[DependsOn(
    typeof(AbpBackgroundWorkersModule),
    typeof(AbpHangfireModule))]
public class AbpBackgroundWorkersHangfireModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton(typeof(HangfirePeriodicBackgroundWorkerAdapter<>));
    }
    
    public async override Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundWorkerOptions>>().Value;
        if (!options.IsEnabled)
        {
            var hangfireOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpHangfireOptions>>().Value;
            hangfireOptions.BackgroundJobServerFactory = CreateOnlyEnqueueJobServer;
        }
        
        await context.ServiceProvider
            .GetRequiredService<IBackgroundWorkerManager>()
            .StartAsync(); 
    }

    public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnPreApplicationInitializationAsync(context));
    }
    
    private BackgroundJobServer CreateOnlyEnqueueJobServer(IServiceProvider serviceProvider)
    {
        serviceProvider.GetRequiredService<JobStorage>();
        return null;
    }
}
