using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EventBus.RabbitMq;

public interface IRabbitMqDistributedEventBus : IDistributedEventBus
{
    void Initialize();
}
