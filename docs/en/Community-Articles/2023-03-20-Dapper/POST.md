# Using Dapper with the ABP Framework

[Dapper](https://github.com/DapperLib/Dapper) is a simple and lightweight object mapper for .NET. A key feature of Dapper is its [high performance](https://github.com/DapperLib/Dapper#performance) compared to other ORMs. In this article, I will show how to use it in your ABP projects.

## When to use Dapper?

In ABP Framework, we suggest to use Dapper in a combination with Entity Framework Core (EF Core) for the following reasons:

* EF Core is much easier to use (you don't need to manually write SQL queries for example)
* EF Core abstracts different DBMS dialects, so it will be easier to change your DBMS later.
* EF Core is better compatible with Object Oriented Programming (OOP) practices and is more type safe to work with. So, EF Core code is more understandable and maintainable.

In most of your use cases, you typically work with one or a few entities where the performance doesn't make much difference. However, there may be certain places in your application where it matters:

* You may work with a lot of entities, so want to query faster.
* You may be performing too many database operations in a single request.
* EF Core may not create an optimized SQL query and you may want to manually write it for a better performance.

In such cases, performance can be critical. Dapper can be a good choice if you want to easily write SQL queries and bind the result to your objects.

