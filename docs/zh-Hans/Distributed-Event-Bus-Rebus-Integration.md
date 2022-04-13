# 分布式事件总线Rebus集成

> 本文解释了 **如何配置[Rebus](http://mookid.dk/category/rebus/)** 做为分布式总线提供程序. 参阅[分布式事件总线文档](Distributed-Event-Bus.md)了解如何使用分布式事件总线系统.

## 安装

使用ABP CLI添加[Volo.Abp.EventBus.Rebus](https://www.nuget.org/packages/Volo.Abp.EventBus.Rebus)NuGet包到你的项目:

* 安装[ABP CLI](https://docs.abp.io/en/abp/latest/CLI),如果你还没有安装.
* 在你想要安装 `Volo.Abp.EventBus.Rebus` 包的 `.csproj` 文件目录打开命令行(终端).
* 运行 `abp add-package Volo.Abp.EventBus.Rebus` 命令.

如果你想要手动安装,安装[Volo.Abp.EventBus.Rebus](https://www.nuget.org/packages/Volo.Abp.EventBus.Rebus) NuGet 包到你的项目然后添加 `[DependsOn(typeof(AbpEventBusRebusModule))]` 到你的项目[模块](Module-Development-Basics.md)类.

## 配置

可以使用配置使用标准的[配置系统](Configuration.md),如[选项](Options.md)类.

`AbpRebusEventBusOptions` 类用于配置事件总线选项.

你可以在你的[模块](Module-Development-Basics.md)的 `PreConfigureServices` 方法配置选项.

**示例: 最小化配置**

```csharp
PreConfigure<AbpRebusEventBusOptions>(options =>
{
    options.InputQueueName = "eventbus";
});
```

Rebus 有很多选项,你可以使用 `AbpRebusEventBusOptions` 的 `Configurer` 属性来配置.

默认事件**存储在内存中**. 参阅[rebus文档](https://github.com/rebus-org/Rebus/wiki/Transport)了解更多信息.

**示例: 配置存储**

````csharp
PreConfigure<AbpRebusEventBusOptions>(options =>
{
    options.InputQueueName = "eventbus";
    options.Configurer = rebusConfigurer =>
    {
        rebusConfigurer.Transport(t => t.UseMsmq("eventbus"));
        rebusConfigurer.Subscriptions(s => s.UseJsonFile(@"subscriptions.json"));
    };
});
````

你可以使用 `AbpRebusEventBusOptions` 的 `Publish` 属性来更改发布方法.

**示例: 配置事件发布**

````csharp
PreConfigure<AbpRebusEventBusOptions>(options =>
{
    options.InputQueueName = "eventbus";
    options.Publish = async (bus, type, data) =>
    {
        await bus.Publish(data);
    };
});
````
