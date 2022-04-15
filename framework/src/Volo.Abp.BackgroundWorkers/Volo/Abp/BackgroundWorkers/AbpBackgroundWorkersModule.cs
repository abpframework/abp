using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers;

[DependsOn(
    typeof(AbpThreadingModule)
    )]
public class AbpBackgroundWorkersModule : AbpModule
{
    private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

    public override Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundWorkerOptions>>().Value;
        if (options.IsEnabled)
        {
            var backgroundWorkerManager = context.ServiceProvider.GetRequiredService<IBackgroundWorkerManager>();

            _cancellationToken.Token.Register(state =>
            {
                AsyncHelper.RunSync(() => backgroundWorkerManager.StopAsync());
            }, backgroundWorkerManager);

            Task.Factory.StartNew(async () => await backgroundWorkerManager.StartAsync(_cancellationToken.Token), TaskCreationOptions.LongRunning);
        }

        return Task.CompletedTask;
    }

    public override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundWorkerOptions>>().Value;
        if (options.IsEnabled)
        {
            _cancellationToken.Cancel();
            _cancellationToken.Dispose();
        }

        return Task.CompletedTask;
    }

    public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnPostApplicationInitializationAsync(context));
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationShutdownAsync(context));
    }
}
