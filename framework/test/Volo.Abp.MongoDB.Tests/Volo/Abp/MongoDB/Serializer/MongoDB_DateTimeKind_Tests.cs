using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Volo.Abp.TestApp.Testing;
using Volo.Abp.Timing;
using Xunit;

namespace Volo.Abp.MongoDB.Serializer
{
    [Collection(MongoTestCollection.Name)]
    public abstract class MongoDB_DateTimeKind_Tests : DateTimeKind_Tests<AbpMongoDbTestModule>, IDisposable
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
}
