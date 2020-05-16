# 示例应用

这些是ABP框架创建的官方示例. 这些示例大部分在[abpframework/abp-samples](https://github.com/abpframework/abp-samples) GitHub 仓库.

### 微服务示例

演示如何基于微服务体系结构构建系统的完整解决方案.

* [示例的文档](Microservice-Demo.md)
* [源码](https://github.com/abpframework/abp/tree/dev/samples/MicroserviceDemo)
* [微服务架构文档](../Microservice-Architecture.md)

### Book Store

一个简单的CRUD应用程序,展示了使用ABP框架开发应用程序的基本原理. 使用不同的技术实现了相同的示例:

* **Book Store: Razor Pages UI & Entity Framework Core**

  * [教程](https://docs.abp.io/en/abp/latest/Tutorials/Part-1?UI=MVC)
  * [源码](https://github.com/abpframework/abp-samples/tree/master/BookStore)

* **Book Store: Angular UI & MongoDB**

  * [教程](https://docs.abp.io/en/abp/latest/Tutorials/Part-1?UI=NG)
  * [源码](https://github.com/abpframework/abp-samples/tree/master/BookStore-Angular-MongoDb)

* **Book Store: Modular application (Razor Pages UI & EF Core)**

  * [源码](https://github.com/abpframework/abp-samples/tree/master/BookStore-Modular)

如果没有Razor Pages & MongoDB 结合,但你可以检查两个文档来理解它,因为DB和UI不会互相影响.

### 其他示例

* **Entity Framework 迁移**: 演示如何将应用程序拆分为多个数据库的解决方案. 每个数据库包含不同的模块.
  * [源码](https://github.com/abpframework/abp-samples/tree/master/DashboardDemo)
  * [EF Core数据库迁移文档](../Entity-Framework-Core-Migrations.md)
* **SignalR Demo**: A simple chat application that allows to send and receive messages among authenticated users.
  * [源码](https://github.com/abpframework/abp-samples/tree/master/SignalRDemo)
  * [SignalR 集成文档](../SignalR-Integration.md)
* **Dashboard Demo**: 一个简单的应用程序,展示了如何在ASP.NET Core MVC UI中使用widget系统.
  * [源码](https://github.com/abpframework/abp-samples/tree/master/DashboardDemo)
  * [Widget 文档](../UI/AspNetCore/Widgets.md)
* **RabbitMQ 事件总线 Demo**: 由两个通过RabbitMQ集成的分布式事件相互通信的应用程序组成的解决方案.
  * [源码](https://github.com/abpframework/abp-samples/tree/master/RabbitMqEventBus)
  * [分布式事件总线文档](../Distributed-Event-Bus.md)
  * [RabbitMQ 分布式事件总线集成文档](../Distributed-Event-Bus-RabbitMQ-Integration.md)
* **自定义认证**: 如何为ASP.NET Core MVC / Razor Pages应用程序自定义身份验证的解决方案.
  * [源码](https://github.com/abpframework/abp-samples/tree/master/aspnet-core/Authentication-Customization)
  * 相关 "[How To](../How-To/Index.md)" 文档:
    * [Azure Active Directory 认证](../How-To/Azure-Active-Directory-Authentication-MVC.md)
    * [自定义登录页面](../How-To/Customize-Login-Page-MVC.md)
    * [自定义 SignIn Manager](../How-To/Customize-SignIn-Manager.md)
* **空的ASP.NET Core应用程序**: 从基本的ASP.NET Core应用程序使用ABP框架.
  * [源码](https://github.com/abpframework/abp-samples/tree/master/BasicAspNetCoreApplication)
  * [文档](../Getting-Started-AspNetCore-Application.md)
* **空的控制台应用程序**: 从基本的控制台应用程序安装ABP框架.
  * [源码](https://github.com/abpframework/abp-samples/tree/master/BasicConsoleApplication)
  * [文档](../Getting-Started-Console-Application.md)