using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;
using Volo.Abp.MultiQueue.Options;
using Volo.Abp.MultiQueue.Subscriber;
using Volo.Abp.Threading;


namespace Volo.Abp.MultiQueue;

[DependsOn(typeof(AbpEventBusModule))]
public class AbpMultiQueueModule : AbpModule
{
    private readonly List<QueueHandlerAttribute> _needSubTopic;
    public AbpMultiQueueModule()
    {
        _needSubTopic = new List<QueueHandlerAttribute>();
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistered((c) =>
        {
            if (typeof(IEventHandler).IsAssignableFrom(c.ImplementationType))
            {
                var attr = c.ImplementationType.GetCustomAttribute<QueueHandlerAttribute>();
                if (attr != null)
                {
                    _needSubTopic.Add(attr);
                }
            }
        });
    }


    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var queueFactory = context.ServiceProvider.GetRequiredService<IAbpMultiQueueFactory>();
        foreach (var item in _needSubTopic)
        {
            var subscriber = queueFactory.GetSubscriber(item.Key);
            if (subscriber != null)
            {
                await subscriber.SubscribeAsync(item.Topic, item.EventType);
            }
            else
            {
                var logger = context.ServiceProvider.GetRequiredService<ILogger<AbpMultiQueueModule>>();
                logger.LogWarning($"Subscriber is null or empty. Key->[{item.Key}]");
            }
        }
    }

    public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
    {
        var optionKeys = QueueOptionsExtension.QueueOptionsTypeMap.Keys;
        if (optionKeys != null && optionKeys.Count > 0)
        {
            foreach (var optionKey in optionKeys)
            {
                var optionType = QueueOptionsExtension.GetOptionType(optionKey);
                if (optionType != null)
                {
                    var type = typeof(IQueueSubscriber<>).MakeGenericType(optionType);
                    var sub = context.ServiceProvider.GetService(type) as IQueueSubscriber;

                    if (sub != null)
                    {
                        sub.Start();
                    }
                }
            }
        }
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        var optionsContainer = context.ServiceProvider.GetRequiredService<IOptions<QueueOptionsContainer>>();
        if (optionsContainer.Value.Options != null && optionsContainer.Value.Options.Count > 0)
        {
            foreach (var item in optionsContainer.Value.Options)
            {
                var optionType = QueueOptionsExtension.GetOptionType(item.Key);
                if (optionType != null)
                {
                    var type = typeof(IQueueSubscriber<>).MakeGenericType(optionType);
                    var sub = context.ServiceProvider.GetService(type) as IQueueSubscriber;

                    if (sub != null)
                    {
                        sub.Stop();
                    }
                }
            }
        }
    }
}
