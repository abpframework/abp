using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.DistributedEvents;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.Oracle.Devart;

[DependsOn(
    typeof(AbpEntityFrameworkCoreModule)
    )]
public class AbpEntityFrameworkCoreOracleDevartModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpSequentialGuidGeneratorOptions>(options =>
        {
            if (options.DefaultSequentialGuidType == null)
            {
                options.DefaultSequentialGuidType = SequentialGuidType.SequentialAsBinary;
            }
        });

        context.Services.AddTransient(typeof(IOracleDbContextEventOutbox<>), typeof(OracleDbContextEventOutbox<>));
        context.Services.AddTransient(typeof(IOracleDbContextEventInbox<>), typeof(OracleDbContextEventInbox<>));
    }
}
