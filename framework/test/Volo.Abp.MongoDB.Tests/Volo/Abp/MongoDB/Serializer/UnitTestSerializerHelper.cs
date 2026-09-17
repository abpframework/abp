using System;
using System.Linq;
using System.Reflection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Volo.Abp.Timing;

namespace Volo.Abp.MongoDB.Serializer;

// MongoDB uses static properties to store the mapping information,
// We must reconfigure it in the new unit test.
public static class UnitTestSerializerHelper
{
    public static void FixSerializers(DateTimeKind? kind)
    {
        foreach (var registeredClassMap in BsonClassMap.GetRegisteredClassMaps())
        {
            foreach (var declaredMemberMap in registeredClassMap.DeclaredMemberMaps.Where(x => x.MemberType == typeof(DateTime) || x.MemberType == typeof(DateTime?)))
            {
                IBsonSerializer serializer = null;
                if (kind != null)
                {
                    serializer = !declaredMemberMap.MemberInfo.IsDefined(typeof(DisableDateTimeNormalizationAttribute), true)
                            ? declaredMemberMap.MemberType == typeof(DateTime?) ? new NullableSerializer<DateTime>().WithSerializer(new AbpMongoDbDateTimeSerializer(kind.Value, false))
                                : new AbpMongoDbDateTimeSerializer(kind.Value, false)
                            : declaredMemberMap.MemberType == typeof(DateTime?) ? new NullableSerializer<DateTime>().WithSerializer(new DateTimeSerializer(DateTimeKind.Unspecified))
                                : new DateTimeSerializer(DateTimeKind.Unspecified);
                }

                var fieldInfo = declaredMemberMap.GetType().GetField("_serializer", BindingFlags.NonPublic | BindingFlags.Instance);
                fieldInfo?.SetValue(declaredMemberMap, serializer);
            }
        }
    }
}
