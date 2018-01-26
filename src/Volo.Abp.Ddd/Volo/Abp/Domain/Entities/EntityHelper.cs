using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace Volo.Abp.Domain.Entities
{
    /// <summary>
    /// Some helper methods for entities.
    /// </summary>
    public static class EntityHelper
    {
        public static bool IsEntity([NotNull] Type type)
        {
            return typeof(IEntity).IsAssignableFrom(type);
        }

        public static bool IsTransient<TPrimaryKey>(IEntity<TPrimaryKey> entity) // TODO: Completely remove IsTransient
        {
            if (EqualityComparer<TPrimaryKey>.Default.Equals(entity.Id, default))
            {
                return true;
            }

            //Workaround for EF Core since it sets int/long to min value when attaching to dbcontext
            if (typeof(TPrimaryKey) == typeof(int))
            {
                return Convert.ToInt32(entity.Id) <= 0;
            }

            if (typeof(TPrimaryKey) == typeof(long))
            {
                return Convert.ToInt64(entity.Id) <= 0;
            }

            return false;
        }

        /// <summary>
        /// Tries to find the primary key type of the given entity type.
        /// May return null if given type does not implement <see cref="IEntity{TPrimaryKey}"/>
        /// </summary>
        [CanBeNull]
        public static Type FindPrimaryKeyType<TEntity>()
            where TEntity : IEntity
        {
            return FindPrimaryKeyType(typeof(TEntity));
        }

        /// <summary>
        /// Tries to find the primary key type of the given entity type.
        /// May return null if given type does not implement <see cref="IEntity{TPrimaryKey}"/>
        /// </summary>
        [CanBeNull]
        public static Type FindPrimaryKeyType([NotNull] Type entityType)
        {
            if (!typeof(IEntity).IsAssignableFrom(entityType))
            {
                throw new AbpException($"Given {nameof(entityType)} is not an entity. It should implement {typeof(IEntity).AssemblyQualifiedName}!");
            }

            foreach (var interfaceType in entityType.GetTypeInfo().GetInterfaces())
            {
                if (interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEntity<>))
                {
                    return interfaceType.GenericTypeArguments[0];
                }
            }

            return null;
        }

        public static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId<TEntity, TPrimaryKey>(TPrimaryKey id)
            where TEntity : IEntity<TPrimaryKey>
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));
            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
            );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
    }
}