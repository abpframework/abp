using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.DistributedEvents;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.PostgreSql
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule)
        )]
    public class AbpEntityFrameworkCorePostgreSqlModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpSequentialGuidGeneratorOptions>(options =>
            {
                if (options.DefaultSequentialGuidType == null)
                {
                    options.DefaultSequentialGuidType = SequentialGuidType.SequentialAsString;
                }
            });

            context.Services.AddTransient(typeof(IPostgreSqlDbContextEventOutbox<>), typeof(PostgreSqlDbContextEventOutbox<>));
            context.Services.AddTransient(typeof(IPostgreSqlDbContextEventInbox<>), typeof(PostgreSqlDbContextEventInbox<>));
        }
    }
}
