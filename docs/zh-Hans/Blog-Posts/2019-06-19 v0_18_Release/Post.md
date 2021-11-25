# ABP CLI,v0.18版本的新模板和其他功能

ABP v0.18已发布, 包含解决的[80+个issue](https://github.com/abpframework/abp/milestone/16?closed=1),[550+次提交](https://github.com/abpframework/abp/compare/0.17.0.0...0.18.0)

## 网站更改

[abp.io](https://abp.io)网站**完全更新**以突出ABP框架的目标和重要功能.文档和博客网址也会更改:

- `abp.io/documents`移至[docs.abp.io](https://docs.abp.io).
- `abp.io/blog`转移到[blog.abp.io](https://blog.abp.io).

## ABP CLI

ABP CLI(命令行界面)是一种新的全局命令行工具,用于执行基于ABP的解决方案的一些常见操作.主要功能是;

* **创建新的应用程序**或模块项目.
* **向应用程序添加新模块**.
* **更新**解决方案中所有与ABP相关的包.

ABP CLI现在是创建新项目的首选方式,你仍然可以从[开始](https://abp.io/get-started)页面下载新项目.

### 用法

使用命令行窗口安装ABP CLI:

```` bash
dotnet tool install -g Volo.Abp.Cli
````

创建一个新应用程序:

```` bash
abp new Acme.BookStore
````

将模块添加到应用程序:

```` bash
abp add-module Volo.Blogging
````

更新解决方案中所有与ABP相关的包:

```` bash
abp update
````

有关详细信息,请参阅[ABP CLI文档](https://docs.abp.io/en/abp/latest/CLI).

## 新模板

在此版本中,我们更新了所有启动模板.主要目标是提供基于领域驱动设计层的更好的启动模板,这些模板还允许创建分层解决方案(Web和API层可以在物理上分开).它还包括针对不同层分开的单元和集成测试项目.

下图显示了MVC应用程序的新启动项目.

![mvc-template-solution](mvc-template-solution.png)

有关详细信息,请参阅[启动模板文档](https://docs.abp.io/en/abp/latest/Startup-Templates/Index).

## 更改日志

以下是此版本附带的一些其他功能和增强功能:

* 新[Volo.Abp.Dapper](https://www.nuget.org/packages/Volo.Abp.Dapper)包.
* 新[Volo.Abp.Specifications](https://www.nuget.org/packages/Volo.Abp.Specifications)包.
* 具有`IDataSeeder`服务和`IDataSeedContributor`接口的新数据种子系统,允许模块化初始数据种子系统.
* 改进了MemoryDB实现,以序列化/反序列化存储在内存中的对象,因此它为单元/集成测试中的数据库模拟提供了更真实的基础结构.
* 为docs模块添加了多语言支持.用于[ABP文档](https://docs.abp.io).

有关此版本中的所有功能,增强功能和错误修正,请参阅[GitHub发行说明](https://github.com/abpframework/abp/releases/tag/0.18.0).

## 路线图

与ABP v1.0版本相关的一件事是.NET Core / ASP.NET Core 3.0版本.根据[.NET核心路线图](https://github.com/dotnet/core/blob/master/roadmap.md),计划于2019年9月发布3.0版本.

ASP.NET Core具有很大的变化和功能.作为一个重大的突破性变化,它将[仅在.NET Core上运行](https://github.com/aspnet/Announcements/issues/324)(删除.net标准支持),因此它不能用于完整.net框架了.

我们已宣布在2019年第二季度发布v1.0.我们应该为v1.0做的主要工作是:

* 填补当前功能的空白.
* 重构和改进当前的API.
* 修复已知的错误.
* 完成文档和教程.

除了我们应该做的工作之外,我们还在考虑等待ASP.NET Core 3.0发布.因为,如果我们在ASP.NET Core 3.0之前发布ABP v1.0,我们将不得不在短时间内再次发布ABP v2.0并放弃v1.0支持.因此,我们正在考虑使用ASP.NET Core 3.0 RC发布ABP v1.0 RC,并将最终发布日期与Microsoft保持一致.

## 想要贡献？

感谢社区对ABP开发的支持.非常感谢.如果你还想参与,请参阅[本指南](https://github.com/abpframework/abp/blob/master/docs/en/Contribution/Index.md)作为开始.
