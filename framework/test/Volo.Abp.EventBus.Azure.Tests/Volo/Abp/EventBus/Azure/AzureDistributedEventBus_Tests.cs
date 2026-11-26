using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using Volo.Abp.AzureServiceBus;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.Tracing;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EventBus.Azure;

public class AzureDistributedEventBus_Tests
{
    private readonly IAzureServiceBusMessageConsumerFactory _consumerFactory;
    private readonly IPublisherPool _publisherPool;
    private readonly TestableAzureDistributedEventBus _bus;
    private readonly AbpAzureEventBusOptions _options;

    public AzureDistributedEventBus_Tests()
    {
        _options = new AbpAzureEventBusOptions
        {
            ConnectionName = "Default",
            SubscriberName = "DefaultSubscriber",
            TopicName = "DefaultTopic"
        };

        _consumerFactory = Substitute.For<IAzureServiceBusMessageConsumerFactory>();
        _consumerFactory.CreateMessageConsumer(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(Substitute.For<IAzureServiceBusMessageConsumer>());

        _publisherPool = Substitute.For<IPublisherPool>();

        _bus = new TestableAzureDistributedEventBus(
            Substitute.For<IServiceScopeFactory>(),
            Substitute.For<ICurrentTenant>(),
            Substitute.For<IUnitOfWorkManager>(),
            Microsoft.Extensions.Options.Options.Create(new AbpDistributedEventBusOptions()),
            Substitute.For<IGuidGenerator>(),
            Substitute.For<IClock>(),
            Microsoft.Extensions.Options.Options.Create(_options),
            Substitute.For<IAzureServiceBusSerializer>(),
            _consumerFactory,
            _publisherPool,
            Substitute.For<IEventHandlerInvoker>(),
            Substitute.For<ILocalEventBus>(),
            Substitute.For<ICorrelationIdProvider>());
    }

    [Fact]
    public void Initialize_WithDefaultOptions_CreatesSingleConsumer()
    {
        _bus.Initialize();

        _consumerFactory.Received(1).CreateMessageConsumer("DefaultTopic", "DefaultSubscriber", "Default");
    }

    [Fact]
    public void Initialize_WithMultipleTopics_CreatesOneConsumerPerTopic()
    {
        _options.Topics = new List<AzureEventBusTopicOptions>
        {
            new() { TopicName = "Orders", SubscriberName = "OrdersSub", ConnectionName = "Conn1" },
            new() { TopicName = "Payments", SubscriberName = "PaymentsSub", ConnectionName = "Conn2" }
        };

        _bus.Initialize();

        _consumerFactory.Received(1).CreateMessageConsumer("Orders", "OrdersSub", "Conn1");
        _consumerFactory.Received(1).CreateMessageConsumer("Payments", "PaymentsSub", "Conn2");
        _consumerFactory.DidNotReceive().CreateMessageConsumer("DefaultTopic", Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public async Task PublishAsync_WithMappedEvent_PublishesToMappedTopic()
    {
        _options.EventTopicMap["OrderCreated"] = "Orders";
        _options.Topics = new List<AzureEventBusTopicOptions>
        {
            new() { TopicName = "Orders", SubscriberName = "OrdersSub", ConnectionName = "Conn1" }
        };

        var sender = Substitute.For<ServiceBusSender>();
        _publisherPool.GetAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(sender);

        await _bus.PublishMessageAsync("OrderCreated", new byte[] { 1, 2, 3 });

        await _publisherPool.Received(1).GetAsync("Orders", "Conn1");
        await sender.Received(1).SendMessageAsync(Arg.Is<ServiceBusMessage>(m => m.Subject == "OrderCreated"));
    }

    [Fact]
    public async Task PublishAsync_WithUnmappedEvent_PublishesToDefaultTopic()
    {
        _options.EventTopicMap["OrderCreated"] = "Orders";

        var sender = Substitute.For<ServiceBusSender>();
        _publisherPool.GetAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(sender);

        await _bus.PublishMessageAsync("SomethingElse", new byte[] { 1, 2, 3 });

        await _publisherPool.Received(1).GetAsync("DefaultTopic", "Default");
    }

    [Fact]
    public async Task PublishManyFromOutboxAsync_GroupsEventsByTopic()
    {
        _options.EventTopicMap["OrderCreated"] = "Orders";
        _options.Topics = new List<AzureEventBusTopicOptions>
        {
            new() { TopicName = "Orders", SubscriberName = "OrdersSub", ConnectionName = "Conn1" }
        };

        var sender = Substitute.For<ServiceBusSender>();
        sender.CreateMessageBatchAsync().Returns(_ => CreateMessageBatch());
        _publisherPool.GetAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(sender);

        var events = new[]
        {
            new OutgoingEventInfo(Guid.NewGuid(), "OrderCreated", new byte[] { 1 }, DateTime.UtcNow),
            new OutgoingEventInfo(Guid.NewGuid(), "OrderCreated", new byte[] { 2 }, DateTime.UtcNow),
            new OutgoingEventInfo(Guid.NewGuid(), "SomethingElse", new byte[] { 3 }, DateTime.UtcNow)
        };

        await _bus.PublishManyFromOutboxAsync(events, new OutboxConfig("Test"));

        // One publisher per distinct topic, one batch sent per topic.
        await _publisherPool.Received(1).GetAsync("Orders", "Conn1");
        await _publisherPool.Received(1).GetAsync("DefaultTopic", "Default");
        await sender.Received(2).SendMessagesAsync(Arg.Any<ServiceBusMessageBatch>());
    }

    // ServiceBusMessageBatch is sealed with no public constructor, so build a
    // real one over an in-memory AMQP batch. No connection is needed: adding
    // messages only serializes them.
    private static ServiceBusMessageBatch CreateMessageBatch()
    {
        var assembly = typeof(ServiceBusMessage).Assembly;
        var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        var converterType = assembly.GetType("Azure.Messaging.ServiceBus.Amqp.AmqpMessageConverter")!;
        var converter = Activator.CreateInstance(converterType, nonPublic: true)!;

        var transportBatchType = assembly.GetType("Azure.Messaging.ServiceBus.Amqp.AmqpMessageBatch")!;
        var transportBatch = transportBatchType.GetConstructors(flags).Single().Invoke(
            new object[] { converter, new CreateMessageBatchOptions { MaxSizeInBytes = 256 * 1024 } });

        var diagnosticsType = assembly.GetType("Azure.Core.Shared.MessagingClientDiagnostics")!;
        var diagnostics = diagnosticsType.GetConstructors(flags).Single().Invoke(
            new object[] { "test", "Microsoft.ServiceBus", "servicebus", "fake.servicebus.windows.net", "Orders" });

        return (ServiceBusMessageBatch)typeof(ServiceBusMessageBatch).GetConstructors(flags).Single().Invoke(
            new object[] { transportBatch, diagnostics });
    }

    private class TestableAzureDistributedEventBus : AzureDistributedEventBus
    {
        public TestableAzureDistributedEventBus(
            IServiceScopeFactory serviceScopeFactory,
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            IOptions<AbpDistributedEventBusOptions> abpDistributedEventBusOptions,
            IGuidGenerator guidGenerator,
            IClock clock,
            IOptions<AbpAzureEventBusOptions> abpAzureEventBusOptions,
            IAzureServiceBusSerializer serializer,
            IAzureServiceBusMessageConsumerFactory messageConsumerFactory,
            IPublisherPool publisherPool,
            IEventHandlerInvoker eventHandlerInvoker,
            ILocalEventBus localEventBus,
            ICorrelationIdProvider correlationIdProvider)
            : base(serviceScopeFactory, currentTenant, unitOfWorkManager, abpDistributedEventBusOptions,
                guidGenerator, clock, abpAzureEventBusOptions, serializer, messageConsumerFactory,
                publisherPool, eventHandlerInvoker, localEventBus, correlationIdProvider)
        {
        }

        public Task PublishMessageAsync(string eventName, byte[] body)
        {
            return PublishAsync(eventName, body, null, null);
        }
    }
}
