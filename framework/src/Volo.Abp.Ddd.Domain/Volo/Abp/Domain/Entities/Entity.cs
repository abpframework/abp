using System;
using System.Collections.Generic;
using System.Reflection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Domain.Entities
{
    /// <inheritdoc/>
    [Serializable]
    public abstract class Entity : IEntity
    {
        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[ENTITY: {GetType().Name}] Keys = {GetKeys().JoinAsString(", ")}";
        }

        public abstract object[] GetKeys();
    }

    /// <inheritdoc cref="IEntity{TKey}" />
    [Serializable]
    public abstract class Entity<TKey> : Entity, IEntity<TKey>
    {
        /// <inheritdoc/>
        public virtual TKey Id { get; protected set; }

        protected Entity()
        {

        }

        protected Entity(TKey id)
        {
            Id = id;
        }

        public bool EntityEquals(object obj)
        {
            if (obj == null || !(obj is Entity<TKey>))
            {
                return false;
            }

            //Same instances must be considered as equal
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            //Transient objects are not considered as equal
            var other = (Entity<TKey>)obj;
            if (EntityHelper.HasDefaultId(this) && EntityHelper.HasDefaultId(other))
            {
                return false;
            }

            //Must have a IS-A relation of types or must be same type
            var typeOfThis = GetType().GetTypeInfo();
            var typeOfOther = other.GetType().GetTypeInfo();
            if (!typeOfThis.IsAssignableFrom(typeOfOther) && !typeOfOther.IsAssignableFrom(typeOfThis))
            {
                return false;
            }

            //Different tenants may have an entity with same Id.
            if (this is IMultiTenant && other is IMultiTenant &&
                this.As<IMultiTenant>().TenantId != other.As<IMultiTenant>().TenantId)
            {
                return false;
            }

            return Id.Equals(other.Id);
        }
        
        public override object[] GetKeys()
        {
            return new object[] {Id};
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[ENTITY: {GetType().Name}] Id = {Id}";
        }
    }
}
