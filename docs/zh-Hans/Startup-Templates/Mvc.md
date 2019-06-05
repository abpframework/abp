# MVC应用程序启动模板

## 介绍

MVC应用程序启动模板是基于[领域驱动设计](../Domain-Driven-Design.md)(DDD)分层(或根据偏好分层)的应用程序结构.

## 如何开始

你要以使用[ABP CLI](../CLI.md)创建基于此启动模板的新项目,或者你也可以在[入门](https://abp.io/get-started)页面创建并下载项目. 在这里我们使用CLI创建新项目.

首先根据[文档](../CLI.md)中的说明安装ABP CLI. 然后使用 `abp new` 命令在空文件夹中创建新解决方案:

````bash
abp new Acme.BookStore -t mvc
````

* `Acme.BookStore` 是解决方案的名称,  命名样式如 *YourCompany.YourProduct*. 更多的命名样式请参阅[CLI文档](../CLI.md).
* 示例中指定了启动模板 (`-t` 或 `--template` 选项). 不过 `mvc` 是默认模板,即使未指定也会创建 `MVC` 的模板项目.

### 指定数据库提供程序

`MVC`模板支持以下数据库提供程序:

- `ef`: Entity Framework Core (默认)
- `mongodb`: MongoDB

使用 `-d` (或 `--database-provider`) 指定数据库提供程序:

````bash
abp new Acme.BookStore -t mvc -d mongodb
````

### 创建分层解决方案

使用 `--tiered` 选项创建分层解决方案, Web与WebApi层在物理上是分开的. 如果未指定,CLI会创建一个分层的解决方案,这个解决方案没有那么复杂,适合大多数场景.

````bash
abp new Acme.BookStore --tiered
````

有关分层的方法,请参阅下面的"分层结构"部分.

## 解决方案结构

根据命令的选项,会创建略有不同的解决方案结构.

### 默认结构

如果未指定选项,你会得到如下所示的解决方案:

![bookstore-visual-studio-solution-v3](../images/bookstore-visual-studio-solution-v3.png)

项目组织在`src`和`test`文件夹中. `src`文件夹包含实际应用程序,该应用程序基于前面提到的[DDD](../Domain-Driven-Design.md)原则进行分层.

--------------------

**TODO: 添加一些图来说明项目之间的依赖关系.**

------------------

下面介绍解决方案中的项目.

#### .Domain 项目

解决方案的领域层. 它主要包含 [实体, 集合根](../Entities.md), [领域服务](../Domain-Services.md), [值类型](../Value-Types.md), [仓储接口](../Repositories.md) 和解决方案的其他领域对象.

例如 `Book` 实体和 `IBookRepository` 接口都适合放在这个项目中.

* 它依赖 `.Domain.Shared` 项目,因为项目中会用到它的一些常量,枚举和定义其他对象.

#### .Domain.Shared 项目

项目包含常量,枚举和其他对象, 这些对象实际上是领域层的一部分,但是解决方案中所有的层/项目中都会使用到.

例如 `BookType` 枚举和 `BookConts` 类 (可能是 `Book` 实体用到的常数字段,像`MaxNameLength`)都适合放在这个项目中.

该项目不会依赖解决方案中的其他项目.

#### .Application.Contracts 项目

项目主要包含 [应用服务](../Application-Services.md) **interfaces** 和应用层的 [数据传输对象](../Data-Transfer-Objects.md) (DTO). 它用于分离应用层的接口和实现. 这种方式可以将接口项目做为约定包共享给客户端.

* 它依赖 `.Domain.Shared` 因为它可能会在应用接口和DTO中使用常量,枚举和其它的共享对象.

#### .Application 项目

项目包含 `.Application.Contracts` 项目的 [应用服务](../Application-Services.md) 接口实现.

* 它依赖 `.Application.Contracts` 项目, 因为它需要实现接口与使用DTO.
* 它依赖 `.Domain` 项目,因为它需要使用领域对象(实体,仓储接口等)执行应用程序逻辑.

#### .EntityFrameworkCore 项目

这是集成EF Core的项目. 它定义了 `DbContext` 并实现 `.Domain` 项目中定义的仓储接口.

* 它依赖 `.Domain` 项目,因为它需要引用实体和仓储接口.

> 只有在你使用了EF Core做为数据库提供程序时,此项目才会可用.

#### .EntityFrameworkCore.DbMigrations 项目

包含解决方案的EF Core数据库迁移. 它有独立的 `DbContext` 来专门管理迁移.

ABP是一个模块化的框架,理想的设计是让每个模块都有自己的 `DbContext` 类. 这时用于迁移的 `DbContext` 就会发挥作用. 它将所有的 `DbContext` 配置统一到单个模型中以维护单个数据库的模式.

需要注意,迁移 `DbContext` 仅用于数据库迁移,而不在*运行时*使用.

* 它依赖 `.EntityFrameworkCore` 项目,因为它重用了应用程序的 `DbContext` 配置 .

> 只有在你使用了EF Core做为数据库提供程序时,此项目才会可用.

#### .DbMigrator 项目

这是一个控制台应用唾弃,它简化了在开发和生产环境执行数据库迁移的操作.当你使用它时;

* 必要时创建数据库.
* 应用数据库迁移.
* 初始化种子数据.

> 这个项目有自己的 `appsettings.json` 文件. 所以如果要更改数据库连接字符串,请记得也要更改此文件.

初始化种子数据很很要,ABP具有模块化的种子数据基础设施. 种子数据的更多信息,请参阅[文档](../Data-Seeding.md).

虽然创建数据库和应用迁移似乎只对关系数据库有用,但即使您选择NoSQL数据库提供程序(如MongoDB),也会生成此项目. 这时,它会为应用程序提供必要的初始数据.

* 它依赖 `.EntityFrameworkCore.DbMigrations` 项目 (针对EF Core),因为它需要访问迁移文件.
* 它依赖 `.Application.Contracts` 项目,因为它需要访问权限定义在初始化种子数据时为管理员用户赋予权限.

#### .HttpApi 项目

用于定义API控制器.

大多数情况下,你不需要手动定义API控制器,因为ABP的[动态API](../AspNetCore/Auto-API-Controllers.md)功能会根据你的应用层自动创建API控制器. 但是,如果你需要编写API控制器,那么它是最合适的地方.

* 它依赖 `.Application.Contracts` 项目,因为它需要注入应用服务接口.

#### .HttpApi.Client 项目

定义C#客户端代理使用解决方案的HttpAPI项目. 可以将上编辑共享给第三方客户端,让它们轻松的在DotNet应用程序中使用你的httiApi.

`.HttpApi.Client.ConsoleTestApp` 项目是一个用于演示客户端代理用法的控制台应用程序.

ABP有[动态 C# API 客户端](../AspNetCore/Dynamic-CSharp-API-Clients.md)功能,所以大多数情况下你不需要手动的创建C#客户端代理.

* 它依赖 `.Application.Contracts` 项目,因为它需要使用应用服务接口和DTO.

#### .Web 项目

包含应用程序的用户界面(UI). 包括Razor页面,javascript文件,css文件,图片等...

* 依赖 `.HttpApi` 项目,因为UI层需要使用解决方案的API和应用服务接口.

> 如果查看 `.Web.csproj` 源码, 你会看到对 `.Application` 和 `.EntityFrameworkCore.DbMigrations` 项目的引用.
>
> 这些引用实际上在开发中并不需要. 因为UI层通常不依赖于EF Core或应用程序实现. 这个启动模板已经为分层部署做好了准备,API层托管在不同与UI层的服务器中.
>
> 但是如果你不选择 `--tiered` 选项, .Web项目会有这些引用,以便能够将Web,Api和应用层托管在单个应用程序站点.
>
> 你可以在表示层中使用领域实体,但是根据DDD的理论,这被认为是一种不好的做法.

#### 测试项目

解决方案有多个测试项目,每一层都会有一个:

* `.Domain.Tests` 用于测试领域层.
* `.Application.Tests` 用于测试应用层.
* `.EntityFrameworkCore.Tests` 用于测试EF Core配置与自定义仓储.
* `.Web.Tests` 用于测试UI.
* `.TestBase` 所有测试项目的基础(共享)项目.

此外,  `.HttpApi.Client.ConsoleTestApp` 是一个控制台应用程序(不是自动化测试项目),它用于演示DotNet应用程序中Http Api的用法.

### 分层结构

TODO

### 其它数据库提供程序

TODO

#### MongoDB

TODO