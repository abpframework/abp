using System.Collections.Generic;
using Volo.Abp.Autofac;
using Volo.Abp.Internal.Telemetry.Activity.Providers;
using Volo.Abp.Modularity;

namespace Volo.Abp.Cli;

[DependsOn(
    typeof(AbpCliCoreModule),
    typeof(AbpAutofacModule)
)]
public class AbpCliModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.RemoveAll(x => x.ImplementationType == typeof(TelemetrySessionInfoEnricher));
    }
}
