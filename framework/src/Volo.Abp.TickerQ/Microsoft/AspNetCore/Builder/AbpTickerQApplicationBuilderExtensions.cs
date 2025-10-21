using Microsoft.Extensions.DependencyInjection;
using TickerQ.DependencyInjection;
using TickerQ.Utilities;
using TickerQ.Utilities.Enums;
using Volo.Abp.TickerQ;

namespace Microsoft.AspNetCore.Builder;

public static class AbpTickerQApplicationBuilderExtensions
{
    public static IApplicationBuilder UseAbpTickerQ(this IApplicationBuilder app, TickerQStartMode qStartMode = TickerQStartMode.Immediate)
    {
        var abpTickerQFunctionProvider = app.ApplicationServices.GetRequiredService<AbpTickerQFunctionProvider>();
        TickerFunctionProvider.RegisterFunctions(abpTickerQFunctionProvider.Functions);
        TickerFunctionProvider.RegisterRequestType(abpTickerQFunctionProvider.RequestTypes);

        app.UseTickerQ(qStartMode);
        return app;
    }
}
