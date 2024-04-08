# Data Access

ABP framework was designed as database agnostic. It can work any type of data source by the help of the [repository](Repositories.md) and [unit of work](Unit-Of-Work.md) abstractions. Currently, the following providers are implemented as official:

* [Entity Framework Core](entity-framework-core/index.md) (works with [various DBMS and providers](https://docs.microsoft.com/en-us/ef/core/providers/).)
* [MongoDB](mongodb/index.md)
* [Dapper](dapper/index.md)

## See Also

* [Connection Strings](../fundamentals/Connection-Strings.md)
* [Data Seeding](../infrastructure/Data-Seeding.md)
* [Data Filtering](../infrastructure/Data-Filtering.md)