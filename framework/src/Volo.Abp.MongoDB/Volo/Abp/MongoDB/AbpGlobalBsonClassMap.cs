using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization;
using Volo.Abp.Data;

namespace Volo.Abp.MongoDB
{
    public static class AbpGlobalBsonClassMap
    {
        private static readonly HashSet<Type> PreConfiguredTypes = new HashSet<Type>();

        /// <summary>
        /// Configure default/base properties for the entity using <see cref="BsonClassMap"/>.
        /// This method runs single time for an <typeparamref name="TEntity"/> for the application lifetime.
        /// Subsequent calls has no effect for the same <typeparamref name="TEntity"/>.
        /// </summary>
        public static void ConfigureDefaults<TEntity>()
        {
            ConfigureDefaults(typeof(TEntity));
        }

        /// <summary>
        /// Configure default/base properties for the entity using <see cref="BsonClassMap"/>.
        /// This method runs single time for an <paramref name="entityType"/> for the application lifetime.
        /// Subsequent calls has no effect for the same <paramref name="entityType"/>.
        /// </summary>
        public static void ConfigureDefaults(Type entityType)
        {
            lock (PreConfiguredTypes)
            {
                if (PreConfiguredTypes.Contains(entityType))
                {
                    return;
                }

                ConfigureDefaultsInternal(entityType);
                PreConfiguredTypes.Add(entityType);
            }
        }

        private static void ConfigureDefaultsInternal(Type entityType)
        {
            var map = new BsonClassMap(entityType);

            map.AutoMap();

            if (entityType.IsAssignableTo<IHasExtraProperties>())
            {
                map.SetExtraElementsMember(
                    new BsonMemberMap(
                        map,
                        entityType.GetMember(nameof(IHasExtraProperties.ExtraProperties))[0]
                    )
                );
            }

            BsonClassMap.RegisterClassMap(map);
        }
    }
}
