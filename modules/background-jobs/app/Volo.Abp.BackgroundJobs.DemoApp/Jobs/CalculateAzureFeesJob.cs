using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs.DemoApp.Jobs;

public class CalculateAzureFeesJobArgs
{
    public string SubscriptionId { get; set; } = default!;
}

public class CalculateAzureFeesJob : AsyncBackgroundJob<CalculateAzureFeesJobArgs>, ITransientDependency
{
    public override Task ExecuteAsync(CalculateAzureFeesJobArgs args)
    {
        // Isolated on the same dedicated fees worker as CalculateAwsFeesJob.
        Logger.LogInformation($"[Azure fees] Calculating fees for subscription '{args.SubscriptionId}'...");
        return Task.CompletedTask;
    }
}
