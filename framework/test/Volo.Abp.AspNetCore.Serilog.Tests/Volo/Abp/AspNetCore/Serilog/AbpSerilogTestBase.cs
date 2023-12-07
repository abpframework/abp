using System.Linq;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Volo.Abp.AspNetCore.App;

namespace Volo.Abp.AspNetCore.Serilog;

public class AbpSerilogTestBase : AbpAspNetCoreTestBase<Program>
{
    protected readonly CollectingSink CollectingSink = new CollectingSink();

    protected override IHost CreateHost(IHostBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Sink(CollectingSink)
            .CreateLogger();

        builder.UseSerilog();
        return base.CreateHost(builder);;
    }

    protected LogEvent GetLogEvent(string text)
    {
        return CollectingSink.Events.FirstOrDefault(m => m.MessageTemplate.Text == text);
    }
}
