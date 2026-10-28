using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Domain.Entities.Events;

public class EntityChangeEventHelper_Tests : AbpIntegratedTest<EntityChangeEventHelper_Tests.TestModule>
{
    private readonly EntityChangeEventHelper _entityChangeEventHelper;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly ILocalEventBus _localEventBus;
    private readonly IDistributedEventBus _distributedEventBus;
    private readonly AbpDistributedEntityEventOptions _options;

    public EntityChangeEventHelper_Tests()
    {
        _entityChangeEventHelper = GetRequiredService<EntityChangeEventHelper>();
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        _localEventBus = GetRequiredService<ILocalEventBus>();
        _distributedEventBus = GetRequiredService<IDistributedEventBus>();
        _options = GetRequiredService<IOptions<AbpDistributedEntityEventOptions>>().Value;
    }

    [Fact]
    public async Task Should_Create_A_New_Created_Event_For_Each_Entity()
    {
        var receivedEvents = new List<EntityCreatedEventData<Student>>();
        using var subscription = _localEventBus.Subscribe<EntityCreatedEventData<Student>>(
            eventData =>
            {
                receivedEvents.Add(eventData);
                return Task.CompletedTask;
            });
        var first = new Student(1);
        var second = new Student(2);

        using var uow = _unitOfWorkManager.Begin();
        _entityChangeEventHelper.PublishEntityCreatedEvent(first);
        _entityChangeEventHelper.PublishEntityCreatedEvent(second);

        receivedEvents.ShouldBeEmpty();
        await uow.CompleteAsync();

        receivedEvents.Count.ShouldBe(2);
        receivedEvents[0].Entity.ShouldBeSameAs(first);
        receivedEvents[1].Entity.ShouldBeSameAs(second);
        receivedEvents[0].ShouldNotBeSameAs(receivedEvents[1]);
    }

    [Fact]
    public async Task Should_Create_A_New_Updated_Event_For_Each_Entity()
    {
        var receivedEvents = new List<EntityUpdatedEventData<Student>>();
        using var subscription = _localEventBus.Subscribe<EntityUpdatedEventData<Student>>(
            eventData =>
            {
                receivedEvents.Add(eventData);
                return Task.CompletedTask;
            });
        var first = new Student(1);
        var second = new Student(2);

        using var uow = _unitOfWorkManager.Begin();
        _entityChangeEventHelper.PublishEntityUpdatedEvent(first);
        _entityChangeEventHelper.PublishEntityUpdatedEvent(second);

        receivedEvents.ShouldBeEmpty();
        await uow.CompleteAsync();

        receivedEvents.Count.ShouldBe(2);
        receivedEvents[0].Entity.ShouldBeSameAs(first);
        receivedEvents[1].Entity.ShouldBeSameAs(second);
        receivedEvents[0].ShouldNotBeSameAs(receivedEvents[1]);
    }

    [Fact]
    public async Task Should_Create_A_New_Deleted_Event_For_Each_Entity()
    {
        var receivedEvents = new List<EntityDeletedEventData<Student>>();
        using var subscription = _localEventBus.Subscribe<EntityDeletedEventData<Student>>(
            eventData =>
            {
                receivedEvents.Add(eventData);
                return Task.CompletedTask;
            });
        var first = new Student(1);
        var second = new Student(2);

        using var uow = _unitOfWorkManager.Begin();
        _entityChangeEventHelper.PublishEntityDeletedEvent(first);
        _entityChangeEventHelper.PublishEntityDeletedEvent(second);

        receivedEvents.ShouldBeEmpty();
        await uow.CompleteAsync();

        receivedEvents.Count.ShouldBe(2);
        receivedEvents[0].Entity.ShouldBeSameAs(first);
        receivedEvents[1].Entity.ShouldBeSameAs(second);
        receivedEvents[0].ShouldNotBeSameAs(receivedEvents[1]);
    }

    [Fact]
    public async Task Should_Create_Distributed_Events_With_The_Mapped_Eto()
    {
        _options.AutoEventSelectors.Add<Student>();
        _options.EtoMappings.Add<Student, PersonEto>();
        var receivedEvents = new List<EntityCreatedEto<PersonEto>>();
        using var subscription = _distributedEventBus.Subscribe<EntityCreatedEto<PersonEto>>(
            eventData =>
            {
                receivedEvents.Add(eventData);
                return Task.CompletedTask;
            });

        using var uow = _unitOfWorkManager.Begin();
        _entityChangeEventHelper.PublishEntityCreatedEvent(new Student(1));
        _entityChangeEventHelper.PublishEntityCreatedEvent(new Student(2));

        receivedEvents.ShouldBeEmpty();
        await uow.CompleteAsync();

        receivedEvents.Count.ShouldBe(2);
        receivedEvents[0].Entity.GetType().ShouldBe(typeof(PersonEto));
        receivedEvents[0].Entity.Id.ShouldBe(1);
        receivedEvents[1].Entity.Id.ShouldBe(2);
        receivedEvents[0].ShouldNotBeSameAs(receivedEvents[1]);
    }

    [Fact]
    public async Task Should_Create_An_Entity_Event_With_A_Params_Constructor()
    {
        var helper = new ParamsEntityChangeEventHelper(
            _unitOfWorkManager,
            GetRequiredService<IEntityToEtoMapper>(),
            GetRequiredService<IOptions<AbpDistributedEntityEventOptions>>())
        {
            LocalEventBus = _localEventBus
        };
        var receivedEntities = new List<Person>();
        using var subscription = _localEventBus.Subscribe<ParamsEntityEventData<Person>>(
            eventData =>
            {
                receivedEntities.Add(eventData.Entity);
                return Task.CompletedTask;
            });
        var student = new Student(42);

        using var uow = _unitOfWorkManager.Begin();
        helper.Publish(student);
        receivedEntities.ShouldBeEmpty();
        await uow.CompleteAsync();

        receivedEntities.Count.ShouldBe(1);
        receivedEntities[0].ShouldBeSameAs(student);
    }

    [Fact]
    public async Task A_Collectible_Entity_Event_Type_Should_Not_Be_Kept_Alive_By_The_Constructor_Cache()
    {
        var handled = false;
        using var subscription = _localEventBus.Subscribe<EntityCreatedEventData<Entity<int>>>(
            eventData =>
            {
                eventData.Entity.ShouldNotBeNull();
                handled = true;
                return Task.CompletedTask;
            });

        var type = await PublishCollectibleEntityAsync(_entityChangeEventHelper, _unitOfWorkManager);

        handled.ShouldBeTrue();
        for (var i = 0; i < 10 && type.IsAlive; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        type.IsAlive.ShouldBeFalse();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static async Task<WeakReference> PublishCollectibleEntityAsync(EntityChangeEventHelper helper, IUnitOfWorkManager manager)
    {
        var builder = AssemblyBuilder
            .DefineDynamicAssembly(new AssemblyName("Collectible" + Guid.NewGuid().ToString("N")), AssemblyBuilderAccess.RunAndCollect)
            .DefineDynamicModule("Collectible")
            .DefineType("CollectibleEntity", TypeAttributes.Public, typeof(Entity<int>));
        builder.DefineDefaultConstructor(MethodAttributes.Public);
        var entityType = builder.CreateType()!;

        using var uow = manager.Begin();
        helper.PublishEntityCreatedEvent(Activator.CreateInstance(entityType)!);
        await uow.CompleteAsync();

        return new WeakReference(typeof(EntityCreatedEventData<>).MakeGenericType(entityType));
    }

    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpDddDomainModule),
        typeof(AbpAutoMapperModule)
    )]
    public class TestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<TestModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<TestModule>(validate: true);
            });
        }
    }

    public class TestAutoMapperProfile : Profile
    {
        public TestAutoMapperProfile()
        {
            CreateMap<Student, PersonEto>();
        }
    }

    public class Person : Entity<int>
    {
        public Person(int id)
            : base(id)
        {
        }
    }

    public class Student : Person
    {
        public Student(int id)
            : base(id)
        {
        }
    }

    public class PersonEto
    {
        public int Id { get; set; }
    }

    public class ParamsEntityEventData<TEntity> : EntityEventData<TEntity>
    {
        public ParamsEntityEventData(params TEntity[] entities)
            : base(entities[0])
        {
        }
    }

    private class ParamsEntityChangeEventHelper : EntityChangeEventHelper
    {
        public ParamsEntityChangeEventHelper(
            IUnitOfWorkManager unitOfWorkManager,
            IEntityToEtoMapper entityToEtoMapper,
            IOptions<AbpDistributedEntityEventOptions> options)
            : base(unitOfWorkManager, entityToEtoMapper, options)
        {
        }

        public void Publish(object entity)
        {
            TriggerEventWithEntity(LocalEventBus, typeof(ParamsEntityEventData<>), entity, entity);
        }
    }
}
