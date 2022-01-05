# 内容管理系统套件模块

此模块为您的应用程序提供内容管理系统 (Content Management System, CMS) 功能. 它提供 **核心构建块** 和完整工作的 **子系统**, 以创建启用 CMS 功能的您自己的网站, 或出于任何目的使用网站中的构建块.

> **此模块目前仅适用于 MVC / Razor 页面 UI**. 虽然没有官方的 Blazor 软件包, 但它也可以在 Blazor 服务器 UI 中工作, 因为实际上 Blazor 服务器 UI 实际上是一个运行在 ASP.NET Core MVC / Razor 页面应用程序的混合型应用程序.

目前提供以下功能:

* 提供 [**页面**](Pages.md) 管理系统来管理具有动态 URL 的动态页面.
* 提供 [**博客**](Blogging.md) 系统来创建发表具有多种博客支持的博客文章.
* 提供 [**标签**](Tags.md) 系统来标记任何资源, 如博客文章.
* 提供 [**评论**](Comments.md) 系统来添加对任何资源的评论功能, 如博客文章或产品评价页面.
* 提供 [**反应**](Reactions.md) 系统来添加对任何资源的反应 (表情符号) 功能, 如博客文章或评论.
* 提供 [**评级**](Ratings.md) 系统来添加对任何资源的评级功能.
* 提供 [**菜单**](Menus.md) 系统来动态管理公共菜单.

点击功能以了解和学习如何去使用它.

所有功能均可单独使用. 如果你禁用了一个功能, 则在 [全局功能](../../Global-Features.md) 系统的帮助下, 该功能会从你的应用程序甚至数据库表中完全消失.

## 预备要求
- 此模块依赖于 [Blob 存储](../../Blob-Storing.md) 模块来保存媒体内容.
> 确保 `BlobStoring` 模块已安装并至少正确地配置了一个提供程序. 请查阅 [文档](../../Blob-Storing.md) 了解更多信息.

- CMS Kit 使用 [分布式缓存](../../Caching.md) 来提高响应速度.
> 强烈建议在分布式/集群部署中为实现数据一致性使用分布式缓存, 如 [Redis](../../Redis-Cache.md).

## 如何安装

可以使用 [ABP CLI](../../CLI.md) 的 `add-module` 命令为解决方案安装模块. 您可以使用以下命令在命令行中安装 CMS Kit 模块:

```bash
abp add-module Volo.CmsKit
```

> 默认情况下, Cms-Kit `GlobalFeature` 被禁用. 因此初始迁移将为空. 所以, 当你使用 EF Core 安装时,你可以添加 `--skip-db-migrations` 命令来跳过迁移. 启用 Cms-Kit 全局功能后, 请添加新的迁移.

安装过程完成后, 在您的解决方案 `Domain.Shared` 项目中打开 `GlobalFeatureConfigurator` 类, 并将以下代码写入 `Configure` 方法中, 以启用 CMS Kit 模块的全部功能.

```csharp
GlobalFeatureManager.Instance.Modules.CmsKit(cmsKit =>
{
    cmsKit.EnableAll();
});
```

你可能更愿意逐个启用这些功能, 而不是启用全部功能. 以下示例仅启用了 [标签](Tags.md) 和 [评论](Comments.md) 功能:

````csharp
GlobalFeatureManager.Instance.Modules.CmsKit(cmsKit =>
{
    cmsKit.Tags.Enable();
    cmsKit.Comments.Enable();
});
````

> 如果你使用 EF Core, 不要忘记添加一个新的迁移并更新你的数据库.

## 软件包

此模块遵循 [模块开发最佳实践指南](https://docs.abp.io/zh-Hans/abp/latest/Best-Practices/Index), 由多个 NuGet 和 NPM 软件包组成. 如果你想了解软件包及其之间的关系, 请参阅指南.

CMS Kit 软件包专为各种使用场景而设计. 如果您查阅了 [CMS Kit 软件包](https://www.nuget.org/packages?q=Volo.CmsKit) 您将看到一些有 `Admin` 和 `Public` 后缀的软件包. 该模块有两个应用程序层, 原因是他们可能被用于不同类型的应用程序. 这些应用程序层仅使用一个领域层.

 - `Volo.CmsKit.Admin.*` 软件包包括管理员 (后台) 应用程序所必须的功能.
 - `Volo.CmsKit.Public.*` 软件包包括被用于用户阅读博客文章和发表评论的公共网站上的功能.
 - `Volo.CmsKit.*` (不带 Admin/Public 后缀) 软件包称为统一包. 统一包分别是添加 Admin 和 Public (相关层的) 软件包的快照. 如果您有一个用于管理和公共网站的单应用程序, 您可以使用这些软件包.

## 内部结构

### 表/集合 前缀&架构

所有表/集合使用 `Cms` 作为默认前缀. 如果需要更改表的前缀或者设置一个架构名称 (如果你的数据库提供程序支持), 请在 `CmsKitDbProperties` 类中设置静态属性.

### 连接字符串

此模块使用 `CmsKit` 作为连接字符串的名称. 如果您未使用此名称定义连接字符串, 它将回退为 `Default` 连接字符串.

有关详细信息, 请参阅 [连接字符串](https://docs.abp.io/en/abp/latest/Connection-Strings) 文档.
