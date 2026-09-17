using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs.DemoApp.Jobs;

public class SendEmailJobArgs
{
    public string To { get; set; } = default!;

    public string Subject { get; set; } = default!;
}

public class SendEmailJob : AsyncBackgroundJob<SendEmailJobArgs>, ITransientDependency
{
    public override Task ExecuteAsync(SendEmailJobArgs args)
    {
        // A fast job. It is NOT configured for a dedicated worker, so the default worker processes it
        // without waiting behind the slow fee-calculation jobs.
        Logger.LogInformation($"[Email] Sending '{args.Subject}' to '{args.To}'...");
        return Task.CompletedTask;
    }
}
