using System;
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