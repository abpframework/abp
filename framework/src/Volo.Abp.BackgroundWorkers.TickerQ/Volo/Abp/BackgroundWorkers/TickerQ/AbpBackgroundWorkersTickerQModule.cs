using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TickerQ.Utilities.Interfaces.Managers;
using TickerQ.Utilities.Models.Ticker;
using Volo.Abp.Modularity;
using Volo.Abp.TickerQ;

namespace Volo.Abp.BackgroundWorkers.TickerQ;

[DependsOn(typeof(AbpBackgroundWorkersModule), typeof(AbpTickerQModule))]
public class AbpBackgroundWorkersTickerQModule : AbpModule
{
    public override async Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var abpTickerQBackgroundWorkersProvider = context.ServiceProvider.GetRequiredService<AbpTickerQBackgroundWorkersProvider>();
        var cronTickerManager = context.ServiceProvider.GetRequiredService<ICronTickerManager<CronTicker>>();
        var abpBackgroundWorkersTickerQOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundWorkersTickerQOptions>>().Value;
        foreach (var backgroundWorker in abpTickerQBackgroundWorkersProvider.BackgroundWorkers)
        {
            var cronTicker = new CronTicker
            {
                Function = backgroundWorker.Value.Function,
                Expression = backgroundWorker.Value.CronExpression
            };

            var config = abpBackgroundWorkersTickerQOptions.GetConfigurationOrNull(backgroundWorker.Value.WorkerType);
            if (config != null)
            {
                cronTicker.Retries = config.Retries ?? cronTicker.Retries;
                cronTicker.RetryIntervals = config.RetryIntervals ?? cronTicker.RetryIntervals;
            }

            await cronTickerManager.AddAsync(cronTicker);
        }
    }
}
