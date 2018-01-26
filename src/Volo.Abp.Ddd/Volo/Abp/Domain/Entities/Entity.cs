using System.Reflection;

namespace Volo.Abp.Domain.Entities
{
    /// <inheritdoc/>
    public abstract class Entity : IEntity
    {
        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[ENTITY: {GetType().Name}]";
        }
    }

    /// <inheritdoc cref="IEntity{TKey}" />
    public abstract class Entity<TKey> : Entity, IEntity<TKey>
    {
        /// <inheritdoc/>
        public virtual TKey Id { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj)
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
            if (EntityHelper.IsTransient(this) && EntityHelper.IsTransient(other))
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

            return Id.Equals(other.Id);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            if (Id == null)
            {
                return 0;
            }

            return Id.GetHashCode();
        }

        public static bool operator ==(Entity<TKey> left, Entity<TKey> right)
        {
            if (Equals(left, null))
            {
                return Equals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[ENTITY: {GetType().Name}] Id = {Id}";
        }
    }
}
