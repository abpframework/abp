using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs.DemoApp.Jobs;

public class CalculateAwsFeesJobArgs
{
    public string AccountId { get; set; } = default!;
}

public class CalculateAwsFeesJob : AsyncBackgroundJob<CalculateAwsFeesJobArgs>, ITransientDependency
{
    public override Task ExecuteAsync(CalculateAwsFeesJobArgs args)
    {
        // A slow, resource-intensive job that is isolated on a dedicated worker (see DemoAppModule).
        Logger.LogInformation($"[AWS fees] Calculating fees for account '{args.AccountId}'...");
        return Task.CompletedTask;
    }
}
