using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers;

[DependsOn(
    typeof(AbpThreadingModule)
    )]
public class AbpBackgroundWorkersModule : AbpModule
{
    public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundWorkerOptions>>().Value;
        if (options.IsEnabled)
        {
            var hostApplicationLifetime = context.ServiceProvider.GetService<IHostApplicationLifetime>();
            var cancellationToken = hostApplicationLifetime?.ApplicationStopping ?? CancellationToken.None;
            await context.ServiceProvider
                .GetRequiredService<IBackgroundWorkerManager>()
                .StartAsync(cancellationToken);
        }
    }

    public async override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundWorkerOptions>>().Value;
        if (options.IsEnabled)
        {
            var hostApplicationLifetime = context.ServiceProvider.GetService<IHostApplicationLifetime>();
            var cancellationToken = hostApplicationLifetime?.ApplicationStopping ?? CancellationToken.None;
            await context.ServiceProvider
                .GetRequiredService<IBackgroundWorkerManager>()
                .StopAsync(cancellationToken);
        }
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationShutdownAsync(context));
    }
}
