using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Volo.Abp.Domain.Entities
{
    [Serializable]
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        [NotMapped] //TODO: Better to handle in EF Core layer, or just use get/set methods instead of a property?
        public virtual ICollection<object> DomainEvents => _domainEvents ?? (_domainEvents = new Collection<object>());

        private ICollection<object> _domainEvents;
    }

    [Serializable]
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
    {
        [NotMapped] //TODO: Better to handle in EF Core layer, or just use get/set methods instead of a property??
        public virtual ICollection<object> DomainEvents => _domainEvents ?? (_domainEvents = new Collection<object>());

        private ICollection<object> _domainEvents;

        protected AggregateRoot()
        {
            
        }

        protected AggregateRoot(TKey id)
            : base(id)
        {

        }
    }
}