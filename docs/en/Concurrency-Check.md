## Concurrency Check

### Introduction

Concurrency Check (also known as **Concurrency Control**) refers to specific mechanisms used to ensure data consistency in presence of concurrent changes (multiple processes or users access or change the same data in a database at the same time).

There are two commonly used concurrency control mechanisms/approaches;
* **Optimistic Locking**: Optimistic Locking is a concurrency control method that does not use record locking. Instead, **Optimistic Locking** allows multiple users to attempt to **update** or **delete** the same record without informing the users that others are also attempting to **update/delete** the record. 

    * If a user successfully updates/deletes the record, the other users need to get the latest changes for the current record to be able to make changes. If any other user attempts to change the record without getting the latest state of it, informed that a conflict exists. 

* **Pessimistic Locking**: Pessimistic Locking prevents simultaneous updates to records. As soon as one user starts to edit a record, it is getting locked. Other users who attempt to update this record are informed that another user has an update in progress. The other users must wait until the first user has finished committing their changes, thereby releasing the record lock. Only then other users can make changes to this record.

### Base Classes & Interfaces for Concurrency Stamp

#### `IHasConcurrencyStamp` Interface

Inheriting the `IHasConcurrencyStamp` interface directly or indirectly, is enough to use concurrency check for your entities and **Optimistic Locking** is applied for ABP modules by default.

```csharp
public interface IHasConcurrencyStamp 
{
    public string ConcurrencyStamp { get; set; }
}
```

#### Base Classes

Most of the **Entity Classes** provided by ABP Framework, inherit from the `IHasConcurrencyStamp` interface.

- AggregateRoot, AggregateRoot<TKey>
- CreationAuditedAggregateRoot, CreationAuditedAggregateRoot<TKey>
- AuditedAggregateRoot, AuditedAggregateRoot<TKey>
- FullAuditedAggregateRoot, FullAuditedAggregateRoot<TKey>
- FullAuditedAggregateRootWithUser<TUser>, FullAuditedAggregateRootWithUser<TKey, TUser>
- AppFullAuditedEntityWithAudited
- AuditedAggregateRootWithUser<TUser>, AuditedAggregateRootWithUser<TKey, TUser>
- CreationAuditedAggregateRootWithUser<TUser>, CreationAuditedAggregateRootWithUser<TKey, TUser>

//TODO: Entity Classes that inherit from IHasConcurrencyStamp interface (FullAuditedAggregateRoot etc.)