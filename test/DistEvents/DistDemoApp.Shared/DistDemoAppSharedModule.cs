using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Volo.Abp.Autofac;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace DistDemoApp
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpDddDomainModule),
        typeof(AbpEventBusModule)
        )]
    public class DistDemoAppSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            context.Services.AddHostedService<DistDemoAppHostedService>();

            Configure<AbpDistributedEntityEventOptions>(options =>
            {
                options.EtoMappings.Add<TodoItem, TodoItemEto>();
                options.AutoEventSelectors.Add<TodoItem>();
            });

            context.Services.AddSingleton<IDistributedLockProvider>(sp =>
            {
                var connection = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
                return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
            });
        }
    }
}
