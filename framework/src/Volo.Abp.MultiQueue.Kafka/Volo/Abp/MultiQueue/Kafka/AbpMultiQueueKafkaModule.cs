using System;
using Volo.Abp.Modularity;
using Volo.Abp.MultiQueue.Publisher;
using Volo.Abp.MultiQueue.Subscriber;

namespace Volo.Abp.MultiQueue.Kafka;

[DependsOn(typeof(AbpMultiQueueModule))]
public class AbpMultiQueueKafkaModule : AbpMultiQueueModuleBase<KafkaQueueOptions>
{
    protected override (Type ServiceType, Type ImplementationType) GetQueuePublisherType(Type optionsType)
    {
        var implementationType = typeof(KafkaQueuePublisher<>).MakeGenericType(optionsType);
        var serviceType = typeof(IQueuePublisher<>).MakeGenericType(optionsType);
        return (serviceType, implementationType);
    }

    protected override (Type ServiceType, Type ImplementationType) GetQueueSubscriberType(Type optionType)
    {
        var implementationType = typeof(KafkaQueueSubscriber<>).MakeGenericType(optionType);
        var serviceType = typeof(IQueueSubscriber<>).MakeGenericType(optionType);
        return (serviceType, implementationType);
    }
}
