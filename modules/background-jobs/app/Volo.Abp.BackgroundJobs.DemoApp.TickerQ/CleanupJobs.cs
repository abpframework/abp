using System;
using System.Threading;
using System.Threading.Tasks;
using TickerQ.Utilities.Models;

namespace Volo.Abp.BackgroundJobs.DemoApp.TickerQ;

public class CleanupJobs
{
    public async Task CleanupLogsAsync(TickerFunctionContext<string> tickerContext, CancellationToken cancellationToken)
    {
        var logFileName = tickerContext.Request;
        Console.WriteLine($"Cleaning up log file: {logFileName} at {DateTime.Now}");
    }
}
