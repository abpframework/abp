using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DistributedEvents;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Modularity;

namespace DistDemoApp
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(AbpEventBusRabbitMqModule),
        typeof(DistDemoAppSharedModule)
    )]
    public class DistDemoAppEfCoreRabbitMqModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<TodoDbContext>(options =>
            {
                options.AddDefaultRepositories();
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer();
            });
            
            Configure<AbpDistributedEventBusOptions>(options =>
            {
                options.Outboxes.Configure(config =>
                {
                    config.UseDbContext<TodoDbContext>();
                });
                
                options.Inboxes.Configure(config =>
                {
                    config.UseDbContext<TodoDbContext>();
                });
            });
        }
    }
}