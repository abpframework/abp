# Data Access

ABP was designed as database agnostic. It can work any type of data source by the help of the [repository](../architecture/domain-driven-design/repositories.md) and [unit of work](../architecture/domain-driven-design/unit-of-work.md) abstractions. Currently, the following providers are implemented as official:

* [Entity Framework Core](./entity-framework-core) (works with [various DBMS and providers](https://docs.microsoft.com/en-us/ef/core/providers/).)
* [MongoDB](./mongodb)
* [Dapper](./dapper)

## See Also

* [Connection Strings](../fundamentals/connection-strings.md)
* [Data Seeding](../infrastructure/data-seeding.md)
* [Data Filtering](../infrastructure/data-filtering.md)