using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace Volo.Abp.OpenIddict.Tokens;

public class TokenCleanupBackgroundWorker : AsyncPeriodicBackgroundWorkerBase
{
    public TokenCleanupBackgroundWorker(
        AbpAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory,
        IOptionsMonitor<TokenCleanupOptions> cleanupOptions)
        : base(timer, serviceScopeFactory)
    {
        timer.Period = cleanupOptions.CurrentValue.CleanupPeriod;
    }

    protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        await workerContext
            .ServiceProvider
            .GetRequiredService<TokenCleanupService>()
            .CleanAsync();
    }
}
