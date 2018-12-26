using MongoDB.Bson.Serialization;
using Volo.Abp.Data;

namespace Volo.Abp.MongoDB
{
    public static class AbpBsonClassMapExtensions
    {
        public static void ConfigureExtraProperties<T>(this BsonClassMap<T> map)
            where T : class, IHasExtraProperties
        {
            map.SetExtraElementsMember(new BsonMemberMap(
                map,
                typeof(T).GetMember(nameof(IHasExtraProperties.ExtraProperties))[0])
            );
        }
    }
}
