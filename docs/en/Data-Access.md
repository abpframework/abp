# Data Access

ABP framework was designed as database agnostic. It can work any type of data source by the help of the [repository](Repositories.md) and [unit of work](Unit-Of-Work.md) abstractions. Currently, the following providers are implemented as official:

* [Entity Framework Core](Entity-Framework-Core.md) (works with [various DBMS and providers](https://docs.microsoft.com/en-us/ef/core/providers/).)
* [MongoDB](MongoDB.md)
* [Dapper](Dapper.md)

## See Also

* [Connection Strings](Connection-Strings.md)
* [Data Seeding](Data-Seeding.md)
* [Data Filtering](Data-Filtering.md)