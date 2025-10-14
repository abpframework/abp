using Microsoft.Extensions.DependencyInjection;
using TickerQ.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.TickerQ;

public class AbpTickerQModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTickerQ(options =>
        {
            options.SetInstanceIdentifier(context.Services.GetApplicationName());
        });
    }
}
