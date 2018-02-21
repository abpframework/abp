using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Volo.Abp.Domain.Entities
{
    /// <inheritdoc cref="IAggregateRoot" />
    public abstract class AggregateRoot : IAggregateRoot
    {
        [NotMapped] //TODO: Better to handle in EF Core layer?
        public virtual ICollection<object> DomainEvents => _domainEvents ?? (_domainEvents = new Collection<object>());

        private ICollection<object> _domainEvents;
    }

    /// <inheritdoc cref="IAggregateRoot{TKey}" />
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
    {
        [NotMapped] //TODO: Better to handle in EF Core layer?
        public virtual ICollection<object> DomainEvents => _domainEvents ?? (_domainEvents = new Collection<object>());

        private ICollection<object> _domainEvents;
    }
}