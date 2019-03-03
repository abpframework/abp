using System;
using MongoDB.Bson.Serialization;
using Volo.Abp.Data;

namespace Volo.Abp.MongoDB
{
    public static class AbpBsonClassMapExtensions
    {
        public static void ConfigureAbpConventions(this BsonClassMap map)
        {
            map.AutoMap();
            map.ConfigureExtraProperties();
        }

        public static void ConfigureExtraProperties<TEntity>(this BsonClassMap<TEntity> map)
            where TEntity : class, IHasExtraProperties
        {
            map.SetExtraElementsMember(new BsonMemberMap(
                map,
                typeof(TEntity).GetMember(nameof(IHasExtraProperties.ExtraProperties))[0])
            );
        }

        /// <summary>
        /// Configures SetExtraElementsMember if the <see cref="BsonClassMap.ClassType"/>
        /// implements the <see cref="IHasExtraProperties"/> interface.
        /// Otherwise, does nothing 
        /// </summary>
        public static void ConfigureExtraProperties(this BsonClassMap map)
        {
            if (map.ClassType.IsAssignableTo<IHasExtraProperties>())
            {
                map.SetExtraElementsMember(
                    new BsonMemberMap(
                        map,
                        map.ClassType.GetMember(nameof(IHasExtraProperties.ExtraProperties))[0]
                    )
                );
            }
        }
    }
}
