using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
using Xunit;

namespace Volo.Abp.EventBus.Local;

public class GenericInheritanceTest : EventBusTestBase
{
    [Fact]
    public async Task Should_Trigger_For_Inherited_Generic_1()
    {
        var triggeredEvent = false;

        LocalEventBus.Subscribe<EntityChangedEventData<Person>>(
            eventData =>
            {
                eventData.Entity.Id.ShouldBe(42);
                triggeredEvent = true;
                return Task.CompletedTask;
            });

        await LocalEventBus.PublishAsync(new EntityUpdatedEventData<Person>(new Person(42)));

        triggeredEvent.ShouldBe(true);
    }

    [Fact]
    public async Task Should_Trigger_For_Inherited_Generic_2()
    {
        var triggeredEvent = false;

        LocalEventBus.Subscribe<EntityChangedEventData<Person>>(
            eventData =>
            {
                eventData.Entity.Id.ShouldBe(42);
                triggeredEvent = true;
                return Task.CompletedTask;
            });

        await LocalEventBus.PublishAsync(new EntityChangedEventData<Student>(new Student(42)));

        triggeredEvent.ShouldBe(true);
    }

    [Fact]
    public async Task Should_Trigger_For_Inherited_Generic_Every_Time()
    {
        var triggeredIds = new List<int>();

        LocalEventBus.Subscribe<EntityChangedEventData<Person>>(
            eventData =>
            {
                triggeredIds.Add(eventData.Entity.Id);
                return Task.CompletedTask;
            });

        await LocalEventBus.PublishAsync(new EntityChangedEventData<Student>(new Student(1)));
        await LocalEventBus.PublishAsync(new EntityChangedEventData<Student>(new Student(2)));
        await LocalEventBus.PublishAsync(new EntityChangedEventData<Student>(new Student(3)));

        triggeredIds.ShouldBe(new[] { 1, 2, 3 });
    }

    [Fact]
    public async Task Should_Trigger_For_Inherited_Generic_With_Multiple_Constructors()
    {
        var triggeredEvent = false;

        LocalEventBus.Subscribe<MultipleConstructorsEventData<Person>>(
            eventData =>
            {
                eventData.Entity.Id.ShouldBe(42);
                triggeredEvent = true;
                return Task.CompletedTask;
            });

        await LocalEventBus.PublishAsync(new MultipleConstructorsEventData<Student>(new Student(42)));

        triggeredEvent.ShouldBe(true);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(2, false)]
    [InlineData(0, true)]
    [InlineData(1, true)]
    [InlineData(2, true)]
    public async Task Should_Trigger_For_Inherited_Generic_With_Params(int tagCount, bool useArrayArgument)
    {
        var tags = new[] { "first", "second" }[..tagCount];
        var student = new Student(42);
        var receivedEvents = new List<ParamsEventData<Person>>();

        using var subscription = LocalEventBus.Subscribe<ParamsEventData<Person>>(
            eventData =>
            {
                receivedEvents.Add(eventData);
                return Task.CompletedTask;
            });

        await LocalEventBus.PublishAsync(new ParamsEventData<Student>(student, useArrayArgument, tags));

        receivedEvents.Count.ShouldBe(1);
        receivedEvents[0].Entity.ShouldBeSameAs(student);
        receivedEvents[0].Tags.ShouldBe(tags);
        if (useArrayArgument)
        {
            receivedEvents[0].Tags.ShouldBeSameAs(tags);
        }
    }

    [Fact]
    public async Task Should_Select_The_Constructor_For_Each_Publish()
    {
        var receivedEvents = new List<MultipleConstructorsEventData<Person>>();
        using var subscription = LocalEventBus.Subscribe<MultipleConstructorsEventData<Person>>(
            eventData =>
            {
                receivedEvents.Add(eventData);
                return Task.CompletedTask;
            });

        await LocalEventBus.PublishAsync(new MultipleConstructorsEventData<Student>(new Student(1)));
        await LocalEventBus.PublishAsync(new MultipleConstructorsEventData<Student>(new Student(2), "updated"));
        await LocalEventBus.PublishAsync(new MultipleConstructorsEventData<Student>(new Student(3)));

        receivedEvents.Count.ShouldBe(3);
        receivedEvents[0].Entity.Id.ShouldBe(1);
        receivedEvents[0].Reason.ShouldBeNull();
        receivedEvents[1].Entity.Id.ShouldBe(2);
        receivedEvents[1].Reason.ShouldBe("updated");
        receivedEvents[2].Entity.Id.ShouldBe(3);
        receivedEvents[2].Reason.ShouldBeNull();
    }

    [Fact]
    public async Task A_Collectible_Event_Type_Should_Not_Be_Kept_Alive_By_The_Constructor_Cache()
    {
        var handled = false;
        using var subscription = LocalEventBus.Subscribe<EntityChangedEventData<Person>>(
            eventData =>
            {
                eventData.Entity.Id.ShouldBe(42);
                handled = true;
                return Task.CompletedTask;
            });

        var type = await PublishCollectibleEventAsync(LocalEventBus);

        handled.ShouldBeTrue();
        for (var i = 0; i < 10 && type.IsAlive; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        type.IsAlive.ShouldBeFalse();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static async Task<WeakReference> PublishCollectibleEventAsync(ILocalEventBus eventBus)
    {
        var builder = AssemblyBuilder
            .DefineDynamicAssembly(new AssemblyName("Collectible" + Guid.NewGuid().ToString("N")), AssemblyBuilderAccess.RunAndCollect)
            .DefineDynamicModule("Collectible")
            .DefineType("CollectibleEvent", TypeAttributes.Public);
        var parameter = builder.DefineGenericParameters("TEntity")[0];
        var baseType = typeof(EntityChangedEventData<>).MakeGenericType(parameter);
        builder.SetParent(baseType);
        var constructor = builder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { parameter });
        var il = constructor.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Call, TypeBuilder.GetConstructor(baseType, typeof(EntityChangedEventData<>).GetConstructors()[0]));
        il.Emit(OpCodes.Ret);
        var definition = builder.CreateType()!;
        var eventType = definition.MakeGenericType(typeof(Student));

        await eventBus.PublishAsync(eventType, Activator.CreateInstance(eventType, new Student(42))!);

        return new WeakReference(definition.MakeGenericType(typeof(Person)));
    }

    public class ParamsEventData<TEntity> : IEventDataWithInheritableGenericArgument
    {
        public TEntity Entity { get; }

        public string[] Tags { get; }

        public bool UseArrayArgument { get; }

        public ParamsEventData(TEntity entity, bool useArrayArgument, params string[] tags)
        {
            Entity = entity;
            UseArrayArgument = useArrayArgument;
            Tags = tags;
        }

        public object[] GetConstructorArgs()
        {
            if (UseArrayArgument)
            {
                return new object[] { Entity!, UseArrayArgument, Tags };
            }

            var args = new object[Tags.Length + 2];
            args[0] = Entity!;
            args[1] = UseArrayArgument;
            Array.Copy(Tags, 0, args, 2, Tags.Length);
            return args;
        }
    }

    public class MultipleConstructorsEventData<TEntity> : IEventDataWithInheritableGenericArgument
    {
        public TEntity Entity { get; }

        public string Reason { get; }

        public MultipleConstructorsEventData(TEntity entity)
        {
            Entity = entity;
        }

        public MultipleConstructorsEventData(TEntity entity, string reason)
            : this(entity)
        {
            Reason = reason;
        }

        public object[] GetConstructorArgs()
        {
            return Reason == null ? new object[] { Entity! } : new object[] { Entity!, Reason };
        }
    }

    public class Person : Entity<int>
    {
        public Person()
        {

        }

        public Person(int id)
         : base(id)
        {

        }
    }

    public class Student : Person
    {
        public Student()
        {

        }

        public Student(int id)
        : base(id)
        {

        }
    }
}
