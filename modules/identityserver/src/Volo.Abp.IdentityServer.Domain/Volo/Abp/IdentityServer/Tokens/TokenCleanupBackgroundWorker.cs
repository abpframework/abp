using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace Volo.Abp.IdentityServer.Tokens;

public class TokenCleanupBackgroundWorker : AsyncPeriodicBackgroundWorkerBase
{
    protected TokenCleanupOptions Options { get; }

    public TokenCleanupBackgroundWorker(
        AbpAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<TokenCleanupOptions> options)
        : base(
            timer,
            serviceScopeFactory)
    {
        Options = options.Value;
        timer.Period = Options.CleanupPeriod;
    }

    protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        await workerContext
            .ServiceProvider
            .GetRequiredService<TokenCleanupService>()
            .CleanAsync();
    }
}
