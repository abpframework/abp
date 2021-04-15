# Concurrency Check

ABP Framework provides concurrency check with repositories by default. 



## Installation

Concurrency check is not a standalone feature. It's part of Repositories and Domain module. So you have to using following two modules:

- `Volo.Abp.Ddd.Domain`

- `Volo.Abp.EntityFrameworkCore` or `Volo.Abp.MongoDB`

Each Db Provider implements concurrency check itself.

## Define Concurrency Stamp

There are two way to use this feature.

- Implementing `IHasConcurrencyStamp` interface to Entity is enough to use concurrency check for that entity.

  ```csharp
  public class Product : Entity<Guid>, IHasConcurrencyStamp
  {
      public string ConcurrencyStamp { get; set; }
  }
  ```

  

- [BasicAggregateRoot](Entities#basicaggregateroot-class) is already implements `IHasConcurrencyStamp`, so inheriting from BasicAggregateRoot also a way to use Concurrency Check.

  ```csharp
  public class Product : BasicAggregateRoot<Guid>
  {
      // Your properties...
  }
  ```

  Any class can be used which derived from **BasicAggregateRoot** such as `AggrageteRoot<T>`, `AuditedAggregateRoot`, `FullAuditedAggregateRoot` , `CreationAuditedAggregateRoot` , `CreationAuditedAggregateRootWithUser`, `FullAuditedAggregateRootWithUser`  and  `AuditedAggregateRootWithUser`.

## Usage
Optimistic concurrency control works with regular database operations such as `Update` & `Delete` with Abp Repositories and if there is a concurrency stamp mismatch, **AbpDbConcurrencyException** will be thrown. 

>  Optimistic concurrency control may not be possible when you use `UpdateManyAsync` and `DeleteManyAsync` methods.

