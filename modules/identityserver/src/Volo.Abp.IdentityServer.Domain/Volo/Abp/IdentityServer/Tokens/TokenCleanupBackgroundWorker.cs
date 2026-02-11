using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Threading;

namespace Volo.Abp.IdentityServer.Tokens;

public class TokenCleanupBackgroundWorker : AsyncPeriodicBackgroundWorkerBase
{
    protected TokenCleanupOptions Options { get; }
    protected IAbpDistributedLock DistributedLock { get; }

    public TokenCleanupBackgroundWorker(
        AbpAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<TokenCleanupOptions> options,
        IAbpDistributedLock distributedLock)
        : base(
            timer,
            serviceScopeFactory)
    {
        DistributedLock = distributedLock;
        Options = options.Value;
        timer.Period = Options.CleanupPeriod;
    }

    protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        await using (var handle = await DistributedLock.TryAcquireAsync(nameof(TokenCleanupBackgroundWorker)))
        {
            Logger.LogInformation($"Lock is acquired for {nameof(TokenCleanupBackgroundWorker)}");

            if (handle != null)
            {
                await workerContext
                    .ServiceProvider
                    .GetRequiredService<TokenCleanupService>()
                    .CleanAsync();

                Logger.LogInformation($"Lock is released for {nameof(TokenCleanupBackgroundWorker)}");
                return;
            }

            Logger.LogInformation($"Handle is null because of the locking for : {nameof(TokenCleanupBackgroundWorker)}");
        }
    }
}
