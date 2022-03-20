## Concurrency Check

### Introduction

Concurrency Check (also known as **Concurrency Control**) refers to specific mechanisms used to ensure data consistency in presence of concurrent changes (multiple processes or users access or change the same data in a database at the same time).

There are two commonly used concurrency control mechanisms/approaches;
* **Optimistic Concurrency Control**: Optimistic Concurrency Control allows multiple users to attempt to **update** the same record without informing the users that others are also attempting to **update** the record. 

    * If a user successfully updates the record, the other users need to get the latest changes for the current record to be able to make changes. 
    * ABP's concurrency check system uses the **Optimistic Concurrency Control**.

* **Pessimistic Concurrency Control**: Pessimistic Concurrency Control prevents simultaneous updates to records and uses a locking mechanism. For more information please see [here](https://www.martinfowler.com/eaaCatalog/pessimisticOfflineLock.html).

### Usage

#### `IHasConcurrencyStamp` Interface

To enable **concurrency control** to your entity class, you should implement the `IHasConcurrencyStamp` interface, directly or indirectly.

```csharp
public interface IHasConcurrencyStamp 
{
    public string ConcurrencyStamp { get; set; }
}
```

* It is the base interface for **concurrency control** and has a just simple property named `ConcurrencyStamp`. 
* While a new record **creating**, if the entity implements the `IHasConcurrencyStamp` interface, ABP Framework sets a unique value to the **ConcurrencyStamp** property automatically.
* While a record **updating**, ABP Framework compares the **ConcurrencyStamp** property of the entity with the provided **ConcurrencyStamp** value by the user and if the values are matched, updates the **ConcurrencyStamp** property with the new unique value automatically. If there is a mismatch, `AbpDbConcurrencyException` is thrown.

**Example: Applying Concurrency Control for the Book Entity**

Implement the `IHasConcurrencyStamp` interface for your entity:

```csharp
public class Book : Entity<Guid>, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }
        
    //...
}
```

Also, implement your output and update DTO classes from the `IHasConcurrencyStamp` interface:

```csharp
public class BookDto : EntityDto<Guid>, IHasConcurrencyStamp 
{
    //...

    public string ConcurrencyStamp { get; set; }
}

public class UpdateBookDto : IHasConcurrencyStamp 
{
    //...

    public string ConcurrencyStamp { get; set; }
}
```

Set the **ConcurrencyStamp** input value to the entity in the **UpdateAsync** method of your application service, for that purpose you can use the `SetConcurrencyStampIfNotNull` method like as below:

```csharp
public class BookAppService : ApplicationService, IBookAppService 
{
    //...

    public virtual async Task<BookDto> UpdateAsync(Guid id, UpdateBookDto input) 
    {
        var book = await BookRepository.GetAsync(id);

        book.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        //set other input values to the entity ...

        await BookRepository.UpdateAsync(book);
    }
}
```

* After that, when multiple users try to update the same record at the same time, concurrency stamp mismatch occurs and `AbpDbConcurrencyException` is thrown.

#### Base Classes

[Aggregate Root](./Entities.md#aggregateroot-class) entity classes already implement the `IHasConcurrencyStamp` interface. So, if you are deriving from one of these base classes, you don't need to manually implement the `IHasConcurrencyStamp` interface:

- `AggregateRoot`, `AggregateRoot<TKey>`
- `CreationAuditedAggregateRoot`, `CreationAuditedAggregateRoot<TKey>`
- `AuditedAggregateRoot`, `AuditedAggregateRoot<TKey>`
- `FullAuditedAggregateRoot`, `FullAuditedAggregateRoot<TKey>`

**Example: Applying Concurrency Control for the Book Entity**

You can inherit your entity from one of [the base classes](#base-classes):

```csharp
public class Book : FullAuditedAggregateRoot<Guid>
{
    //...
}
```

Then, you can implement your output and update DTO classes from the `IHasConcurrencyStamp` interface:

```csharp
public class BookDto : EntityDto<Guid>, IHasConcurrencyStamp 
{
    //...

    public string ConcurrencyStamp { get; set; }
}

public class UpdateBookDto : IHasConcurrencyStamp 
{
    //...

    public string ConcurrencyStamp { get; set; }
}
```

Set the **ConcurrencyStamp** input value to the entity in the **UpdateAsync** method of your application service, for that purpose you can use the `SetConcurrencyStampIfNotNull` method like as below:

```csharp
public class BookAppService : ApplicationService, IBookAppService 
{
    //...

    public virtual async Task<BookDto> UpdateAsync(Guid id, UpdateBookDto input) 
    {
        var book = await BookRepository.GetAsync(id);

        book.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        //set other input values to the entity ...

        await BookRepository.UpdateAsync(book);
    }
}
```

After that, when multiple users try to update the same record at the same time, concurrency stamp mismatch occurs and `AbpDbConcurrencyException` is thrown. You can either handle the exception manually or let the ABP Framework handle it for you. 

ABP Framework shows a user-friendly error message like in the image below, if you don't handle the exception manually.

![Optimistic Concurrency](./images/optimistic-concurrency.png)