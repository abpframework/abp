using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Volo.Abp.Autofac;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DistributedEvents;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.EventBus.Boxes;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Modularity;

namespace DistDemoApp
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(AbpAutofacModule),
        typeof(AbpEventBusRabbitMqModule),
        typeof(AbpEventBusBoxesModule)
    )]
    public class DistDemoAppModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            
            context.Services.AddHostedService<MyProjectNameHostedService>();
            
            context.Services.AddAbpDbContext<TodoDbContext>(options =>
            {
                options.AddDefaultRepositories();
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer();
            });
            
            Configure<AbpDistributedEntityEventOptions>(options =>
            {
                options.EtoMappings.Add<TodoItem, TodoItemEto>();
                options.AutoEventSelectors.Add<TodoItem>();
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

            context.Services.AddSingleton<IDistributedLockProvider>(sp =>
            {
                var connection = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
                return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
            });
        }
    }
}