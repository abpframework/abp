# 时钟

使用时间和[时区](https://en.wikipedia.org/wiki/Time_zone)总是很棘手,尤其是当你需要构建供**不同时区**的用户使用的**全局系统**时.

ABP提供了一个基本的基础结构,使其变得容易并在可能的情况下自动进行处理. 本文档涵盖了与时间和时区相关的ABP框架服务和系统.

> 如果你正在创建在单个时区区域运行的本地应用程序,则可能不需要这些系统. 但也建议使用本文中介绍的 `IClock` 服务.

## IClock

`DateTime.Now` 返回带有**服务器本地日期和时间**的 `DateTime` 对象. `DateTime` 对象**不存储时区信息**. 因此你无法知道此对象中存储的**绝对日期和时间**. 你只能做一些**假设**,例如假设它是在UTC+05时区创建的. 当你此值保存到数据库中并稍后读取,或发送到**不同时区**的客户端时,事情就变得特别复杂.

解决此问题的一种方法是始终使用 `DateTime.UtcNow` 并将所有 `DateTime` 对象假定为UTC时间. 在这种情况下你可以在需要时将其转换为目标客户端的时区.

`IClock` 在获取当前时间的同时提供了一种抽象,你可以在应用程序中的单个点上控制日期时间的类型(UTC或本地时间).

**示例: 获取当前时间**

````csharp
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace AbpDemo
{
    public class MyService : ITransientDependency
    {
        private readonly IClock _clock;

        public MyService(IClock clock)
        {
            _clock = clock;
        }

        public void Foo()
        {
            //Get the current time!
            var now = _clock.Now;
        }
    }
}
````

* 当你需要获取当前时间时注入 `IClock` 服务. 常用的服务基类(如ApplicationService)已经注入并且做为基类属性提供,所以你可以直接使用 `Clock`.
* 使用 `Now` 属性获取当前时间.

> 在大多数情况下 `IClock` 是你需要在应用程序中了解和使用的唯一服务.

### Clock 选项

`AbpClockOptions` 是用于设置时钟种类的[选项](Options.md)类.

**示例: 使用 UTC Clock**

````csharp
Configure<AbpClockOptions>(options =>
{
    options.Kind = DateTimeKind.Utc;
});
````

在你的[模块](Module-Development-Basics.md)的 `ConfigureServices` 方法添加以上内容.

> 默认 `Kind` 是 `Unspecified`,实际上使时钟不存在. 如果要利用Clock系统，要么使用 `Utc` 或 `Local`.

### DateTime 标准化

`IClock` 的其他重要功能是规范化 `DateTime` 对象.

**示例用法 :**

````csharp
DateTime dateTime = ...; //Get from somewhere
var normalizedDateTime = Clock.Normalize(dateTime)
````

`Normalize` 方法的工作原理如下:

* 如果当前时钟为UTC,并且给定的 `DateTime` 为本地时间,将给定的 `DateTime` 转换为UTC(通过使用 `DateTime.ToUniversalTime()` 方法).
* 如果当前时钟是本地的,并且给定的 `DateTime` 是UTC,将给定的 `DateTime` 转换为本地时间(通过使用 `DateTime.ToUniversalTime()` 方法).
* 如果未指定给定的 `DateTime` 的 `Kind`,将给定的 `DateTime` 的 `Kind`(使用 `DateTime.SpecifyKind(...)` 方法)设置为当前时钟的 `Kind`.

当获取的 `DateTime` 不是由 `IClock` 创建且可能与当前Clock类型不兼容的时候,ABP框架会使用 `Normalize` 方法. 例如;

* ASP.NET Core MVC模型绑定中的 `DateTime` 类型绑定.
* 通过[Entity Framework Core](Entity-Framework-Core.md)将数据保存到数据库或从数据库读取数据.
* 在[JSON反序列化](Json.md)上使用 `DateTime` 对象.

#### DisableDateTimeNormalization Attribute

`DisableDateTimeNormalization` attribute可用于禁用所需类或属性的规范化操作.

### 其他 IClock 属性

除了 `Now`, `IClock` 服务还具有以下属性:

* `Kind`: 返回当前使用的时钟类型(`DateTimeKind.Utc`, `DateTimeKind.Local` 或 `DateTimeKind.Unspecified`)的 `DateTimeKind`.
* `SupportsMultipleTimezone`: 如果当前时间是UTC,则返回 `true`.

## 时区

本节介绍与管理时区有关的ABP框架基础结构

### 时区设置

ABP框架定义了一个名为 `Abp.Timing.Timezone` 的**设置**,可用于为应用程序的用户,[租户](Multi-Tenancy.md)或全局设置和获取时区. 默认值为 `UTC`.

参阅[设置系统]了解更多关于设置系统.

### ITimezoneProvider

`ITimezoneProvider` 是一个服务,可将[Windows时区ID](https://support.microsoft.com/en-us/help/973627/microsoft-time-zone-index-values)值简单转换为[Iana时区名称](https://www.iana.org/time-zones)值,反之亦然. 它还提供了获取这些时区列表与获取具有给定名称的 `TimeZoneInfo` 的方法.

它已使用[TimeZoneConverter](https://github.com/mj1856/TimeZoneConverter)库实现.