using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Volo.Abp.Reflection;

namespace Volo.Abp.Domain.Entities
{
    /// <summary>
    /// Some helper methods for entities.
    /// </summary>
    public static class EntityHelper
    {
        public static bool IsEntity([NotNull] Type type)
        {
            return ReflectionHelper.IsAssignableToGenericType(type, typeof (IEntity<>));
        }

        /// <summary>
        /// Default implementation of <see cref="Entity{TPrimaryKey}.IsTransient"/>.
        /// 
        /// This method is not used normally if given entity is derived from <see cref="Entity{TPrimaryKey}"/>.
        /// Just directly call <see cref="Entity{TPrimaryKey}.IsTransient"/>.
        /// 
        /// This method is exists to help developers who want to directly implement <see cref="IEntity{TPrimaryKey}.IsTransient"/>
        /// but want to use default IsTransient implementation as a shortcut.
        /// </summary>
        public static bool IsTransient<TPrimaryKey>(IEntity<TPrimaryKey> entity)
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

        public static Type GetPrimaryKeyType<TEntity>()
        {
            return GetPrimaryKeyType(typeof (TEntity));
        }

        /// <summary>
        /// Gets primary key type of given entity type
        /// </summary>
        public static Type GetPrimaryKeyType([NotNull] Type entityType)
        {
            foreach (var interfaceType in entityType.GetTypeInfo().GetInterfaces())
            {
                if (interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof (IEntity<>))
                {
                    return interfaceType.GenericTypeArguments[0];
                }
            }

            throw new AbpException("Can not find primary key type of given entity type: " + entityType + ". Be sure that this entity type implements " + typeof(IEntity<>).AssemblyQualifiedName);
        }
    }
}