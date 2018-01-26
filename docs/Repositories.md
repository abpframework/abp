## Repositories

"*Mediates between the domain and data mapping layers using a collection-like interface for accessing domain objects*" (Martin Fowler).

Repositories, in practice, are used to perform database operations for domain objects (see [Entities](Entities.md)). Generally, a separated repository is used for each **aggregate root** or entity.

### Generic Repositories

ABP can provide a default generic repository for each aggregate root or entity. You can [inject](Dependency-Injection.md) `IRepository<TEntity, TKey>` into your service and perform standard CRUD operations. Example usage:

