using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TickerQ.DependencyInjection;
using TickerQ.Utilities;
using TickerQ.Utilities.Enums;
using Volo.Abp.TickerQ;

namespace Microsoft.Extensions.Hosting;

public static class AbpTickerQApplicationBuilderExtensions
{
    public static IHost UseAbpTickerQ(this IHost app, TickerQStartMode qStartMode = TickerQStartMode.Immediate)
    {
        var abpTickerQFunctionProvider = app.Services.GetRequiredService<AbpTickerQFunctionProvider>();
        TickerFunctionProvider.RegisterFunctions(abpTickerQFunctionProvider.Functions);
        TickerFunctionProvider.RegisterRequestType(abpTickerQFunctionProvider.RequestTypes);

        app.UseTickerQ(qStartMode);
        return app;
    }
}
