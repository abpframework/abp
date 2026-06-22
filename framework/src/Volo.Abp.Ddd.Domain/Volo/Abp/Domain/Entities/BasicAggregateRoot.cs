using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain.Entities;

[Serializable]
public abstract class BasicAggregateRoot : Entity,
    IAggregateRoot,
    IGeneratesDomainEvents
{
    private ICollection<DomainEventRecord>? _distributedEvents;
    private ICollection<DomainEventRecord>? _localEvents;

    public virtual IEnumerable<DomainEventRecord> GetLocalEvents()
    {
        return _localEvents ?? Array.Empty<DomainEventRecord>();
    }

    public virtual IEnumerable<DomainEventRecord> GetDistributedEvents()
    {
        return _distributedEvents ?? Array.Empty<DomainEventRecord>();
    }

    public virtual void ClearLocalEvents()
    {
        _localEvents?.Clear();
    }

    public virtual void ClearDistributedEvents()
    {
        _distributedEvents?.Clear();
    }

    protected virtual void AddLocalEvent(object eventData)
    {
        _localEvents ??= new Collection<DomainEventRecord>();
        _localEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }

    protected virtual void AddDistributedEvent(object eventData)
    {
        _distributedEvents ??= new Collection<DomainEventRecord>();
        _distributedEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }
}

[Serializable]
public abstract class BasicAggregateRoot<TKey> : Entity<TKey>,
    IAggregateRoot<TKey>,
    IGeneratesDomainEvents
{
    private ICollection<DomainEventRecord>? _distributedEvents;
    private ICollection<DomainEventRecord>? _localEvents;

    protected BasicAggregateRoot()
    {

    }

    protected BasicAggregateRoot(TKey id)
        : base(id)
    {

    }

    public virtual IEnumerable<DomainEventRecord> GetLocalEvents()
    {
        return _localEvents ?? Array.Empty<DomainEventRecord>();
    }

    public virtual IEnumerable<DomainEventRecord> GetDistributedEvents()
    {
        return _distributedEvents ?? Array.Empty<DomainEventRecord>();
    }

    public virtual void ClearLocalEvents()
    {
        _localEvents?.Clear();
    }

    public virtual void ClearDistributedEvents()
    {
        _distributedEvents?.Clear();
    }

    protected virtual void AddLocalEvent(object eventData)
    {
        _localEvents ??= new Collection<DomainEventRecord>();
        _localEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }

    protected virtual void AddDistributedEvent(object eventData)
    {
        _distributedEvents ??= new Collection<DomainEventRecord>();
        _distributedEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }
}
