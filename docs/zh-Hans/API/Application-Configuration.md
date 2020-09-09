# 应用程序配置端点

ABP框架提供了一个预构建的标准端点,其中包含一些有关应用程序/服务的有用信息. 这里是此端点的一些基本信息的列表:

* [本地化](../Localization.md)值, 支持应用程序的当前语言.
* 当前用户可用和已授予的[策略](../Authorization.md)(权限).
* 当前用户的[设置](../Settings.md)值.
* 关于[当前用户](../CurrentUser.md)的信息 (如 id 和用户名).
* 关于当前[租户](../Multi-Tenancy.md)的信息 (如 id 和名称).
* 当前用户的[时区](../Timing.md)信息和应用程序的[时钟](../Timing.md)类型.

## HTTP API

如果您导航到基于ABP框架的web应用程序或HTTP服务的 `/api/abp/application-configuration` URL, 你可以得到JSON对象形式配置. 该端点对于创建应用程序的客户端很有用.

## Script

对于ASP.NET Core MVC(剃刀页)应用程序,同样的配置值在JavaScript端也可用. `/Abp/ApplicationConfigurationScript` 是基于上述HTTP API自动生成的脚本的URL.

参阅 [JavaScript API文档](../UI/AspNetCore/JavaScript-API/Index.md) 了解关于ASP.NET Core UI.

其他UI类型提供相关平台的本地服务. 例如查看[Angular UI本地化文档](../UI/Angular/Localization.md)来学习如何使用这个端点公开的本地化值.