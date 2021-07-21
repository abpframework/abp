using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Persistence.InMem;
using Rebus.Transport.InMem;

namespace Volo.Abp.EventBus.Rebus
{
    public class AbpRebusEventBusOptions
    {
        [NotNull]
        public string InputQueueName { get; set; }

        [NotNull]
        public Action<RebusConfigurer> Configurer
        {
            get => _configurer;
            set => _configurer = Check.NotNull(value, nameof(value));
        }
        private Action<RebusConfigurer> _configurer;

        [NotNull]
        public Func<IBus, Type, object, Task> Publish
        {
            get => _publish;
            set => _publish = Check.NotNull(value, nameof(value));
        }
        private Func<IBus, Type, object, Task> _publish;

        public AbpRebusEventBusOptions()
        {
            _publish = DefaultPublish;
            _configurer = DefaultConfigure;
        }

        private async Task DefaultPublish(IBus bus, Type eventType, object eventData)
        {
            await bus.Advanced.Routing.Send(InputQueueName, eventData);
        }

        private void DefaultConfigure(RebusConfigurer configure)
        {
            configure.Subscriptions(s => s.StoreInMemory());
            configure.Transport(t => t.UseInMemoryTransport(new InMemNetwork(), InputQueueName));
        }
    }
}
