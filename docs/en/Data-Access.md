# Data Access

ABP framework was designed as database agnostic, it can work any type of data source by the help of the [repository](Repositories.md) and [unit of work](Unit-Of-Work.md) abstractions. 

However, currently the following providers are implements:

* [Entity Framework Core](Entity-Framework-Core.md) (works with [various DBMS and providers](https://docs.microsoft.com/en-us/ef/core/providers/?tabs=dotnet-core-cli).)
* [MongoDB](MongoDB.md)
* [Dapper](Dapper.md)

More providers might be added in the next releases.