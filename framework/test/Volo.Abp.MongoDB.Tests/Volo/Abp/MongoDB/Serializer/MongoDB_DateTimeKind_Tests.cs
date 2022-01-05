using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Shouldly;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.Testing;
using Volo.Abp.Timing;
using Xunit;

namespace Volo.Abp.MongoDB.Serializer;

[Collection(MongoTestCollection.Name)]
public abstract class MongoDB_DateTimeKind_Tests : DateTimeKind_Tests<AbpMongoDbTestModule>
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        // MongoDB uses static properties to store the mapping information,
        // We must reconfigure it in the new unit test.
        foreach (var registeredClassMap in BsonClassMap.GetRegisteredClassMaps())
        {
            var frozen = registeredClassMap.GetType().BaseType?.GetField("_frozen", BindingFlags.NonPublic | BindingFlags.Instance);
            frozen?.SetValue(registeredClassMap, false);

            foreach (var declaredMemberMap in registeredClassMap.DeclaredMemberMaps)
            {
                var serializer = declaredMemberMap.GetSerializer();
                switch (serializer)
                {
                    case AbpMongoDbDateTimeSerializer dateTimeSerializer:
                        dateTimeSerializer.SetDateTimeKind(Kind);
                        break;
                    case NullableSerializer<DateTime> nullableSerializer:
                        {
                            var lazySerializer = nullableSerializer.GetType()
                                ?.GetField("_lazySerializer", BindingFlags.NonPublic | BindingFlags.Instance)
                                ?.GetValue(serializer)?.As<Lazy<IBsonSerializer<DateTime>>>();

                            if (lazySerializer?.Value is AbpMongoDbDateTimeSerializer dateTimeSerializer)
                            {
                                dateTimeSerializer.SetDateTimeKind(Kind);
                            }
                            break;
                        }
                }
            }

            frozen?.SetValue(registeredClassMap, true);
        }
    }
}

public class DateTimeKindTests_Unspecified : MongoDB_DateTimeKind_Tests
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        Kind = DateTimeKind.Unspecified;
        services.Configure<AbpClockOptions>(x => x.Kind = Kind);
        base.AfterAddApplication(services);
    }
}

public class DateTimeKindTests_Local : MongoDB_DateTimeKind_Tests
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        Kind = DateTimeKind.Local;
        services.Configure<AbpClockOptions>(x => x.Kind = Kind);
        base.AfterAddApplication(services);
    }
}

public class DateTimeKindTests_Utc : MongoDB_DateTimeKind_Tests
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        Kind = DateTimeKind.Utc;
        services.Configure<AbpClockOptions>(x => x.Kind = Kind);
        base.AfterAddApplication(services);
    }
}

[Collection(MongoTestCollection.Name)]
public class DisableDateTimeKindTests : TestAppTestBase<AbpMongoDbTestModule>
{
    protected IPersonRepository PersonRepository { get; }

    public DisableDateTimeKindTests()
    {
        PersonRepository = GetRequiredService<IPersonRepository>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpMongoDbOptions>(x => x.UseAbpClockHandleDateTime = false);
        base.AfterAddApplication(services);
    }

    [Fact]
    public async Task DateTime_Kind_Should_Be_Normalized_By_MongoDb_Test()
    {
        var personId = Guid.NewGuid();
        await PersonRepository.InsertAsync(new Person(personId, "bob lee", 18)
        {
            Birthday = DateTime.Parse("2020-01-01 00:00:00"),
            LastActive = DateTime.Parse("2020-01-01 00:00:00"),
        }, true);

        var person = await PersonRepository.GetAsync(personId);

        person.ShouldNotBeNull();
        person.CreationTime.Kind.ShouldBe(DateTimeKind.Utc);

        person.Birthday.ShouldNotBeNull();
        person.Birthday.Value.Kind.ShouldBe(DateTimeKind.Utc);
    }
}
