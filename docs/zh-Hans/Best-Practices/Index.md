## 模块开发最佳实践 & 约定

### 介绍

这篇文档描述了想要满足以下规范的**模块**的**最佳实践**与**约定**:

* 开发应用**领域驱动设计**模式的最佳实践的模块.
* 开发 **DBMS 与 ORM 独立** 的模块.
* 开发可用作 **远程服务 / 微服务** 的模块, 并可以集成到 **单体** 应用程序中.

本指南主要用于 **应用程序** 开发.

### 指南

* 总体
  * [模块架构](Module-Architecture.md)
* 领域层
  * [实体](Entities.md)
  * [仓储](Repositories.md)
  * [领域服务](Domain-Services.md)
* 应用程序层
  * [应用程序服务](Application-Services.md)
  * [数据传输对象](Data-Transfer-Objects.md)
* 数据访问
  * [Entity Framework Core 集成](Entity-Framework-Core-Integration.md)
  * [MongoDB 集成](MongoDB-Integration.md)