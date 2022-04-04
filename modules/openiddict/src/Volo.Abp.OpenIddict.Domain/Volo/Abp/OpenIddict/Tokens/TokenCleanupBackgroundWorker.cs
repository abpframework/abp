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
        /* TODO: Should we use distributed locking here?
         * Because, multiple instances of the application may work in parallel, and only one
         * of them should work in a time. If you can't obtain the lock, it is good to wait for a longer time.
         * I suggest to check https://github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.EventBus/Volo/Abp/EventBus/Distributed/InboxProcessor.cs
         * for a good implementation, then apply a similar principle here.
         */
        await workerContext
            .ServiceProvider
            .GetRequiredService<TokenCleanupService>()
            .CleanAsync();
    }
}
