using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace Volo.Abp.OpenIddict.Tokens
{
    public class OpenIddictTokenCleanupBackgroundWorker : AsyncPeriodicBackgroundWorkerBase
    {
        protected OpenIddictCleanupOptions Options { get; }

        public OpenIddictTokenCleanupBackgroundWorker(
            AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<OpenIddictCleanupOptions> options)
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
                .GetRequiredService<IOpenIddictTokenManager>()
                .PruneAsync(System.DateTimeOffset.UtcNow);
        }
    }
}
