using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Tracing;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EventBus.Distributed;

public class LocalDistributedEventBus_MultiTenancy_Test : LocalDistributedEventBusTestBase
{
    private readonly ICurrentTenant _currentTenant;

    public LocalDistributedEventBus_MultiTenancy_Test()
    {
        _currentTenant = GetRequiredService<ICurrentTenant>();
    }

    [Fact]
    public void Should_Only_Propagate_The_Tenant_Of_Dynamic_Events()
    {
        var tenantId = Guid.NewGuid();
        var eventBus = ActivatorUtilities.CreateInstance<TestableLocalDistributedEventBus>(ServiceProvider);

        using (_currentTenant.Change(tenantId))
        {
            eventBus.GetTenantIdToPropagatePublic(typeof(DynamicEventData), new DynamicEventData("test", new { Value = 1 })).ShouldBe(tenantId);
            eventBus.GetTenantIdToPropagatePublic(typeof(MySimpleEventData), new MySimpleEventData(1)).ShouldBeNull();
        }
    }

    [Fact]
    public async Task Should_Restore_The_Publish_Tenant_When_A_Dynamic_Event_Is_Sent_From_The_Outbox()
    {
        var tenantId = Guid.NewGuid();
        var eventName = "TestEvent-" + Guid.NewGuid().ToString("N");
        Guid? observedTenantId = null;
        var handled = false;

        using var subscription = DistributedEventBus.Subscribe(eventName,
            new SingleInstanceHandlerFactory(new ActionEventHandler<DynamicEventData>(_ =>
            {
                handled = true;
                observedTenantId = _currentTenant.Id;
                return Task.CompletedTask;
            })));

        var outgoingEvent = new OutgoingEventInfo(
            Guid.NewGuid(),
            eventName,
            JsonSerializer.SerializeToUtf8Bytes(new { Value = 1 }),
            DateTime.Now);

        using (_currentTenant.Change(tenantId))
        {
            outgoingEvent.SetTenantId(_currentTenant.Id);
        }

        _currentTenant.Id.ShouldBeNull();

        await GetRequiredService<LocalDistributedEventBus>().PublishFromOutboxAsync(outgoingEvent, new OutboxConfig("Default"));

        handled.ShouldBeTrue();
        observedTenantId.ShouldBe(tenantId);
    }

    [Fact]
    public void Should_Fail_When_The_Tenant_Id_Header_Is_Not_A_Guid()
    {
        EventBusTenantIdHelper.Parse(null).ShouldBeNull();
        EventBusTenantIdHelper.Parse(" ").ShouldBeNull();

        Should.Throw<AbpException>(() => EventBusTenantIdHelper.Parse("not-a-guid"));
    }

    public class TestableLocalDistributedEventBus : LocalDistributedEventBus
    {
        public TestableLocalDistributedEventBus(
            IServiceScopeFactory serviceScopeFactory,
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            IOptions<AbpDistributedEventBusOptions> abpDistributedEventBusOptions,
            IGuidGenerator guidGenerator,
            IClock clock,
            IEventHandlerInvoker eventHandlerInvoker,
            ILocalEventBus localEventBus,
            ICorrelationIdProvider correlationIdProvider)
            : base(
                serviceScopeFactory,
                currentTenant,
                unitOfWorkManager,
                abpDistributedEventBusOptions,
                guidGenerator,
                clock,
                eventHandlerInvoker,
                localEventBus,
                correlationIdProvider)
        {
        }

        public Guid? GetTenantIdToPropagatePublic(Type eventType, object eventData)
        {
            return GetTenantIdToPropagate(eventType, eventData);
        }
    }
}
