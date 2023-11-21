# Performance Optimization
Application overall performance is determined by several factors such as:

* Execution times of database queries
* Execution times of application backend code
* Amount of data that has to be sent to the application frontend and/or inside (micro)services
* Overall speed and occupancy of hardware, which provides the runtime for an application
* Network connectivity speed and responsiveness (latency) on end devices (in case of web application or application which consumes REST or other type of API)

In abp framework, all .NET code performance techniques can be used combined with abp framework-specific features.

# Database
## Caching
To improve performance on the database layer (regardless of database type), it's recommended to use caching wherever possible and safe and as long as possible.

Leverage:

* [Entity Cache](Entity-Cache.md)
* [Redis Cache](Redis-Cache.md)

## Denormalization techniques and JSON (RDBMS)
Using [denormalization](https://en.wikipedia.org/wiki/Denormalization) techniques can (significantly) reduce querying execution times.
Querying such pieces of data from different parts of application should be read from the location where the query is the most efficient (performance-wise).

**Writing the same piece of data multiple times inside different SQL tables or even databases consumes more disk space and requires careful domain logic to insert and update the same data across all locations inside a database (databases) as a transactional unit of work.**

Use denormalization techniques in conjunction with JSON capabilities (No-SQL capabilities) of modern RDBMS, which can extend denormalization even further.

Further reading:
* [JSON data in Microsoft SQL Server](https://learn.microsoft.com/en-us/sql/relational-databases/json/json-data-sql-server)
* [JSON in PostgreSQL](https://www.postgresql.org/docs/current/functions-json.html)
* [JSON Mapping in Npgsql (EF Core)](https://www.npgsql.org/efcore/mapping/json.html)

## Database sharding
The smaller the database, the less data is for querying and this also means lower execution times.
* In multitenant applications, it is recommended to use per-tenant database or multiple databases per tenant (logging db, data db,...).
Each tenant database can be hosted inside different hardware performance tier (cost-effectiveness).
* In a microservice environment, it's recommended to use database-per-microservice or even database-per-tenant-per-microservice
It is possible to achieve described sharding in abp framework:<br>
[Connection Strings](Connection-Strings.md)<br>
[Multitenancy](Multi-Tenancy.md)<br>
[Entity Framework Core Migrations](Entity-Framework-Core-Migrations.md) (Look [Using Multiple Databases](Entity-Framework-Core-Migrations.md#using-multiple-databases) section for migrations)

## Database partitioning (RDBMS)
Most modern RDBMS offers horizontal (and also vertical) [partitioning](https://en.wikipedia.org/wiki/Partition_(database)).
When querying performance is not good enough because of large tables, it's recommended to create horizontal partitions.
* [Partitions in Microsoft SQL Server](https://learn.microsoft.com/en-us/sql/relational-databases/partitions/partitioned-tables-and-indexes)
* [Partitions in PostgreSQL](https://www.postgresql.org/docs/current/ddl-partitioning.html)

## Database indexing, partitioning, denormalization, and sharding techniques
**Database indexing, partitioning, denormalization, and sharding can be used in conjunction.**

## Entity Framework Core
There are some general recommendations for writing efficient queries using EF Core, which are [described here](https://learn.microsoft.com/en-us/ef/core/performance/efficient-querying).

When maximum performance is needed (complex queries, very frequent queries,...), using [Compiled queries](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/ef/language-reference/compiled-queries-linq-to-entities) and/or [SQL queries](https://learn.microsoft.com/en-us/ef/core/querying/sql-queries) can improve performance.

# Application layer (backend)
* [Microsoft ASP.NET Core Best Practices](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/best-practices) covers most topics regarding performance.
Best Practices is part of [Microsoft ASP.NET Core Performance](https://learn.microsoft.com/en-us/aspnet/core/performance/overview) documentation.

* Use [Cached Service Providers](https://docs.abp.io/en/abp/latest/Dependency-Injection#cached-service-providers) (ICachedServiceProvider or ITransientCachedServiceProvider) to optimize dependency injection.

* For computationally intensive tasks, which can leverage parallel code use [Task Parallel Library](https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/task-parallel-library-tpl) (TPL) from Microsoft (available from .NET Framework 4.0 onwards).

# Frontend
## ASP.NET Core MVC (Razor Pages)
### Faster page loading
* Using [Bundling & Minifications](AspNetCore/Bundling-Minification.md) feature of abp framework wherever possible
* Using [Static JavaScript Client Proxies](UI/AspNetCore/Static-JavaScript-Proxies.md)
* Avoid longer loading times with blank pages with minimal server-side rendering and transfer loading on javascript level (ajax) with loading spinners for specific GUI elements which require more time to get data from backend

# Network
This section applies to communication between frontend (MVC, Angular,...) and backend and also backend to databases and/or external API services.

* Reduce the number of calls via the network as much as possible (e.g. usage of API methods that return an array of entities,...)
* Use modern compression algorithms ([Brotli](https://en.wikipedia.org/wiki/Brotli),...) wherever possible
* Make calls in parallel if possible (usage of [HTTP/2](https://en.wikipedia.org/wiki/HTTP/2) or newer protocol for frontend-backed communication and parallel calls from application backend to external services)
* Prefer lighter data formats (e.g. JSON over XML, [Protobuf](https://en.wikipedia.org/wiki/Protocol_Buffers) over JSON) for fewer data size transfers and/or faster serialization/deserialization
* Prefer vector-based data formats for graphics elements instead of raster graphics wherever possible