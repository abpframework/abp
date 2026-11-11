using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Shouldly;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Xunit;

namespace Volo.Abp.EventBus;

public class EventHandlerInvoker_Tests : EventBusTestBase
{
    private readonly IEventHandlerInvoker _eventHandlerInvoker;

    public EventHandlerInvoker_Tests()
    {
        _eventHandlerInvoker = GetRequiredService<IEventHandlerInvoker>();
    }

    [Fact]
    public async Task Should_Invoke_LocalEventHandler_With_MyEventData()
    {
        var localHandler = new MyLocalEventHandler();
        var eventData = new MyEventData();

        await _eventHandlerInvoker.InvokeAsync(localHandler, eventData, eventData.GetType());

        localHandler.MyEventDataCount.ShouldBe(2);
        localHandler.EntityChangedEventDataCount.ShouldBe(0);
        localHandler.EntityChangedEventDataCount.ShouldBe(0);
    }

    [Fact]
    public async Task Should_Invoke_LocalEventHandler_Created_And_Changed_Once()
    {
        var localHandler = new MyLocalEventHandler();
        var eventData = new EntityCreatedEventData<MyEntity>(new MyEntity());

        await _eventHandlerInvoker.InvokeAsync(localHandler, eventData, eventData.GetType());
        await _eventHandlerInvoker.InvokeAsync(localHandler, eventData, typeof(EntityChangedEventData<MyEntity>));

        localHandler.MyEventDataCount.ShouldBe(0);
        localHandler.EntityChangedEventDataCount.ShouldBe(1);
        localHandler.EntityChangedEventDataCount.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Invoke_DistributedEventHandler_With_MyEventData()
    {
        var localHandler = new MyDistributedEventHandler();
        var eventData = new MyEventData();

        await _eventHandlerInvoker.InvokeAsync(localHandler, eventData, eventData.GetType());

        localHandler.MyEventDataCount.ShouldBe(1);
        localHandler.EntityCreatedCount.ShouldBe(0);
    }

    [Fact]
    public async Task Should_Invoke_DistributedEventHandler_With_EntityCreatedEto()
    {
        var localHandler = new MyDistributedEventHandler();
        var eventData = new EntityCreatedEto<MyEntity>(new MyEntity());

        await _eventHandlerInvoker.InvokeAsync(localHandler, eventData, eventData.GetType());

        localHandler.MyEventDataCount.ShouldBe(0);
        localHandler.EntityCreatedCount.ShouldBe(1);
    }

    [Fact]
    public async Task Proxies_Sharing_A_Full_Name_Should_Each_Be_Invoked_By_What_They_Implement()
    {
        // Two dynamic modules can each name a proxy of the same interface the same way.
        var localOnly = (ILocalEventHandler<MyEventData>)new ProxyGenerator(new DefaultProxyBuilder())
            .CreateInterfaceProxyWithTarget(typeof(ILocalEventHandler<MyEventData>), new LocalOnlyEventHandler());
        var both = new BothEventHandler();
        var localAndDistributed = (ILocalEventHandler<MyEventData>)new ProxyGenerator(new DefaultProxyBuilder())
            .CreateInterfaceProxyWithTarget(typeof(ILocalEventHandler<MyEventData>), new[] { typeof(IDistributedEventHandler<MyEventData>) }, both);
        localOnly.GetType().FullName.ShouldBe(localAndDistributed.GetType().FullName);
        localOnly.GetType().ShouldNotBe(localAndDistributed.GetType());

        await _eventHandlerInvoker.InvokeAsync(localOnly, new MyEventData(), typeof(MyEventData));
        await _eventHandlerInvoker.InvokeAsync(localAndDistributed, new MyEventData(), typeof(MyEventData));

        both.LocalCount.ShouldBe(1);
        both.DistributedCount.ShouldBe(1);
    }

    [Fact]
    public async Task A_Collectible_Handler_Type_Should_Not_Be_Kept_Alive_By_The_Invoker_Cache()
    {
        var type = await UseCollectibleHandlerAsync(_eventHandlerInvoker);

        for (var i = 0; i < 10 && type.IsAlive; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        type.IsAlive.ShouldBeFalse();
    }

    [Fact]
    public async Task A_Collectible_Event_Type_A_Handler_Cannot_Handle_Should_Not_Be_Kept_Alive_By_The_Invoker_Cache()
    {
        var type = await UseCollectibleEventTypeAsync(_eventHandlerInvoker);

        for (var i = 0; i < 10 && type.IsAlive; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        type.IsAlive.ShouldBeFalse();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static async Task<WeakReference> UseCollectibleEventTypeAsync(IEventHandlerInvoker invoker)
    {
        var eventType = AssemblyBuilder
            .DefineDynamicAssembly(new AssemblyName("Collectible" + Guid.NewGuid().ToString("N")), AssemblyBuilderAccess.RunAndCollect)
            .DefineDynamicModule("Collectible")
            .DefineType("CollectibleEvent", TypeAttributes.Public)
            .CreateType()!;

        await Should.ThrowAsync<AbpException>(() => invoker.InvokeAsync(new MyLocalEventHandler(), Activator.CreateInstance(eventType)!, eventType));

        return new WeakReference(eventType);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static async Task<WeakReference> UseCollectibleHandlerAsync(IEventHandlerInvoker invoker)
    {
        var handlerInterface = typeof(ILocalEventHandler<MyEventData>);
        var builder = AssemblyBuilder
            .DefineDynamicAssembly(new AssemblyName("Collectible" + Guid.NewGuid().ToString("N")), AssemblyBuilderAccess.RunAndCollect)
            .DefineDynamicModule("Collectible")
            .DefineType("CollectibleHandler", TypeAttributes.Public, typeof(object), new[] { handlerInterface });
        builder.DefineDefaultConstructor(MethodAttributes.Public);
        var method = builder.DefineMethod(
            nameof(ILocalEventHandler<MyEventData>.HandleEventAsync),
            MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot,
            typeof(Task),
            new[] { typeof(MyEventData) });
        var il = method.GetILGenerator();
        il.Emit(OpCodes.Call, typeof(Task).GetProperty(nameof(Task.CompletedTask))!.GetMethod!);
        il.Emit(OpCodes.Ret);
        builder.DefineMethodOverride(method, handlerInterface.GetMethod(nameof(ILocalEventHandler<MyEventData>.HandleEventAsync))!);
        var type = builder.CreateType()!;

        await invoker.InvokeAsync((IEventHandler)Activator.CreateInstance(type)!, new MyEventData(), typeof(MyEventData));

        return new WeakReference(type);
    }

    [Fact]
    public async Task A_Handler_That_Provides_Its_Interface_At_Run_Time_Should_Be_Invoked()
    {
        var handler = new RunTimeInterfaceEventHandler();

        await _eventHandlerInvoker.InvokeAsync(handler, new MyEventData(), typeof(MyEventData));

        handler.Count.ShouldBe(1);
    }

    public class RunTimeInterfaceEventHandler : IEventHandler, IDynamicInterfaceCastable
    {
        public int Count { get; set; }

        public bool IsInterfaceImplemented(RuntimeTypeHandle interfaceType, bool throwIfNotImplemented)
        {
            return interfaceType.Equals(typeof(ILocalEventHandler<MyEventData>).TypeHandle);
        }

        public RuntimeTypeHandle GetInterfaceImplementation(RuntimeTypeHandle interfaceType)
        {
            return typeof(IRunTimeLocalEventHandler).TypeHandle;
        }
    }

    [DynamicInterfaceCastableImplementation]
    public interface IRunTimeLocalEventHandler : ILocalEventHandler<MyEventData>
    {
        Task ILocalEventHandler<MyEventData>.HandleEventAsync(MyEventData eventData)
        {
            ((RunTimeInterfaceEventHandler)this).Count++;
            return Task.CompletedTask;
        }
    }

    public class LocalOnlyEventHandler : ILocalEventHandler<MyEventData>
    {
        public Task HandleEventAsync(MyEventData eventData)
        {
            return Task.CompletedTask;
        }
    }

    public class BothEventHandler : ILocalEventHandler<MyEventData>, IDistributedEventHandler<MyEventData>
    {
        public int LocalCount { get; private set; }

        public int DistributedCount { get; private set; }

        Task ILocalEventHandler<MyEventData>.HandleEventAsync(MyEventData eventData)
        {
            LocalCount++;
            return Task.CompletedTask;
        }

        Task IDistributedEventHandler<MyEventData>.HandleEventAsync(MyEventData eventData)
        {
            DistributedCount++;
            return Task.CompletedTask;
        }
    }

    public class MyEventData
    {
    }

    public class MyEntity : Entity<Guid>
    {

    }

    public class MyDistributedEventHandler : IDistributedEventHandler<MyEventData>,
        IDistributedEventHandler<EntityCreatedEto<MyEntity>>
    {
        public int MyEventDataCount { get; set; }
        public int EntityCreatedCount { get; set; }

        public Task HandleEventAsync(MyEventData eventData)
        {
            MyEventDataCount++;
            return Task.CompletedTask;
        }

        public Task HandleEventAsync(EntityCreatedEto<MyEntity> eventData)
        {
            EntityCreatedCount++;
            return Task.CompletedTask;
        }
    }

    public class MyLocalEventHandler : ILocalEventHandler<MyEventData>,
        IDistributedEventHandler<MyEventData>,
        IDistributedEventHandler<EntityCreatedEventData<MyEntity>>,
        IDistributedEventHandler<EntityChangedEventData<MyEntity>>
    {
        public int MyEventDataCount { get; set; }
        public int EntityCreatedEventDataCount { get; set; }
        public int EntityChangedEventDataCount { get; set; }

        public Task HandleEventAsync(MyEventData eventData)
        {
            MyEventDataCount++;
            return Task.CompletedTask;
        }

        public Task HandleEventAsync(EntityCreatedEventData<MyEntity> eventData)
        {
            EntityCreatedEventDataCount++;
            return Task.CompletedTask;
        }

        public Task HandleEventAsync(EntityChangedEventData<MyEntity> eventData)
        {
            EntityChangedEventDataCount++;
            return Task.CompletedTask;
        }
    }
}
