using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EventBus.Distributed
{
    public class LocalDistributedEventBus_Test : LocalDistributedEventBusTestBase
    {
        [Fact]
        public async Task Should_Call_Handler_AndDispose()
        {
            DistributedEventBus.Subscribe<MySimpleEventData, MySimpleDistributedTransientEventHandler>();

            await DistributedEventBus.PublishAsync(new MySimpleEventData(1));
            await DistributedEventBus.PublishAsync(new MySimpleEventData(2));
            await DistributedEventBus.PublishAsync(new MySimpleEventData(3));

            Assert.Equal(3, MySimpleDistributedTransientEventHandler.HandleCount);
            Assert.Equal(3, MySimpleDistributedTransientEventHandler.DisposeCount);
        }

        [Fact]
        public async Task Should_Change_TenantId_If_EventData_Is_MultiTenant()
        {
            var tenantId = Guid.NewGuid();

            DistributedEventBus.Subscribe<MySimpleEventData>(GetRequiredService<MySimpleDistributedSingleInstanceEventHandler>());

            await DistributedEventBus.PublishAsync(new MySimpleEventData(3, tenantId));

            Assert.Equal(tenantId, MySimpleDistributedSingleInstanceEventHandler.TenantId);
        }

        [Fact]
        public async Task Should_Change_TenantId_If_Generic_EventData_Is_MultiTenant()
        {
            var tenantId = Guid.NewGuid();

            DistributedEventBus.Subscribe<EntityCreatedEto<MySimpleEventData>>(GetRequiredService<MySimpleDistributedSingleInstanceEventHandler>());

            await DistributedEventBus.PublishAsync(new MySimpleEventData(3, tenantId));

            Assert.Equal(tenantId, MySimpleDistributedSingleInstanceEventHandler.TenantId);
        }

        [Fact]
        public async Task Should_Get_TenantId_From_EventEto_Extra_Property()
        {
            var tenantId = Guid.NewGuid();

            DistributedEventBus.Subscribe<MySimpleEto>(GetRequiredService<MySimpleDistributedSingleInstanceEventHandler>());

            await DistributedEventBus.PublishAsync(new MySimpleEto
            {
                Properties =
                {
                    {"TenantId", tenantId.ToString()}
                }
            });

            Assert.Equal(tenantId, MySimpleDistributedSingleInstanceEventHandler.TenantId);
        }

        [Fact]
        public async Task Event_Should_Published_On_UnitOfWorkComplete()
        {
            var id = 0;
            DistributedEventBus.Subscribe<MySimpleEventData>(data =>
            {
                id = data.Value;
                return Task.CompletedTask;
            });

            var unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
            using (var uow = unitOfWorkManager.Begin())
            {
                await DistributedEventBus.PublishAsync(new MySimpleEventData(3), onUnitOfWorkComplete: true, useOutbox: false);
            }
            id.ShouldBe(0);

            using (var uow = unitOfWorkManager.Begin())
            {
                await DistributedEventBus.PublishAsync(new MySimpleEventData(3), onUnitOfWorkComplete: true, useOutbox: false);
                await uow.CompleteAsync();
            }
            id.ShouldBe(3);

            id = 0;
            using (var uow = unitOfWorkManager.Begin())
            {
                await DistributedEventBus.PublishAsync(new MySimpleEventData(3), onUnitOfWorkComplete: false, useOutbox: false);
            }
            id.ShouldBe(3);
        }

        [Fact]
        public async Task Event_Should_Published_On_UnitOfWorkComplete_UseOutbox()
        {
            var id = 0;
            DistributedEventBus.Subscribe<MySimpleEventData>(data =>
            {
                id = data.Value;
                return Task.CompletedTask;
            });

            var unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
            using (var uow = unitOfWorkManager.Begin())
            {
                await DistributedEventBus.PublishAsync(new MySimpleEventData(3), onUnitOfWorkComplete: false, useOutbox: true);
            }
            id.ShouldBe(0);

            using (var uow = unitOfWorkManager.Begin())
            {
                await DistributedEventBus.PublishAsync(new MySimpleEventData(3), onUnitOfWorkComplete: false, useOutbox: true);
                await uow.CompleteAsync();
            }
            id.ShouldBe(3);

            id = 0;
            using (var uow = unitOfWorkManager.Begin())
            {
                await DistributedEventBus.PublishAsync(new MySimpleEventData(3), onUnitOfWorkComplete: false, useOutbox: false);
            }
            id.ShouldBe(3);
        }
    }
}
