# Data Access

## Database Providers

ABP framework was designed as database agnostic. It can work any type of data source by the help of the [repository](Repositories.md) and [unit of work](Unit-Of-Work.md) abstractions. However, currently the following providers are implemented:

* [Entity Framework Core](Entity-Framework-Core.md) (works with [various DBMS and providers](https://docs.microsoft.com/en-us/ef/core/providers/).)
* [MongoDB](MongoDB.md)
* [Dapper](Dapper.md)

More providers will be added in the future.

## See Also

* [Connection Strings](Connection-Strings.md)