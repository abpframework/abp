# 数据访问

ABP框架被设计为与数据库无关, 它通过[仓储](Repositories.md)和[工作单元](Unit-Of-Work.md)抽象处理来自任何类型的数据源.

目前实现了以下数据库访问提供程序:

* [Entity Framework Core](Entity-Framework-Core.md) (与各种[DBMS和提供程序](https://docs.microsoft.com/zh-cn/ef/core/providers/?tabs=dotnet-core-cli)一起使用.)
* [MongoDB](MongoDB.md)
* [Dapper](Dapper.md)

## 另请参阅

* [连接字符串](Connection-Strings.md)
* [种子数据](Data-Seeding.md)
* [数据过滤](Data-Filtering.md)