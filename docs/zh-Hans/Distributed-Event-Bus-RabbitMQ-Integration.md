# 分布式事件总线RabbitMQ集成

> 本文解释了 **如何配置[RabbitMQ](https://www.rabbitmq.com/)** 做为分布式总线提供程序. 参阅[分布式事件总线文档](Distributed-Event-Bus.md)了解如何使用分布式事件总线系统.

## 安装

使用ABP CLI添加[Volo.Abp.EventBus.RabbitMQ](https://www.nuget.org/packages/Volo.Abp.EventBus.RabbitMQ)NuGet包到你的项目:

* 安装[ABP CLI](https://docs.abp.io/en/abp/latest/CLI),如果你还没有安装.
* 在你想要安装 `Volo.Abp.EventBus.RabbitMQ` 包的 `.csproj` 文件目录打开命令行(终端).
* 运行 `abp add-package Volo.Abp.EventBus.RabbitMQ` 命令.

如果你想要手动安装,安装[Volo.Abp.EventBus.RabbitMQ](https://www.nuget.org/packages/Volo.Abp.EventBus.RabbitMQ) NuGet 包到你的项目然后添加 `[DependsOn(typeof(AbpEventBusRabbitMqModule))]` 到你的项目[模块](Module-Development-Basics.md)类.

## 配置

可以使用配置使用标准的[配置系统](Configuration.md),如 `appsettings.json` 文件,或[选项](Options.md)类.

### `appsettings.json` 文件配置

这是配置RabbitMQ设置最简单的方法. 它也非常强大,因为你可以使用[由AspNet Core支持](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)的任何其他配置源(如环境变量).

**示例：最小化配置与默认配置连接到本地的RabbitMQ服务器**

````json
{
  "RabbitMQ": {
    "EventBus": {
      "ClientName": "MyClientName",
      "ExchangeName": "MyExchangeName"
    }
  }
}
````

* `ClientName` 是应用程序的名称,用于RabbitMQ的**队列名称**.
* `ExchangeName` 是 **交换机名称**.

参阅[RabbitMQ文档](https://www.rabbitmq.com/dotnet-api-guide.html#exchanges-and-queues)更好的了解这些选项.

#### 连接

如果需要连接到本地主机以外的另一台服务器,需要配置连接属性.

**示例: 指定主机名 (如IP地址)**

````json
{
  "RabbitMQ": {
    "Connections": {
      "Default": {
        "HostName": "123.123.123.123"
      }
    },
    "EventBus": {
      "ClientName": "MyClientName",
      "ExchangeName": "MyExchangeName"
    }
  }
}
````

允许定义多个连接. 在这种情况下,你可以指定用于事件总线的连接.

**示例: 声明两个连接并将其中一个用于事件总线**

````json
{
  "RabbitMQ": {
    "Connections": {
      "Default": {
        "HostName": "123.123.123.123"
      },
      "SecondConnection": {
        "HostName": "321.321.321.321"
      }
    },
    "EventBus": {
      "ClientName": "MyClientName",
      "ExchangeName": "MyExchangeName",
      "ConnectionName": "SecondConnection"
    }
  }
}
````

这允许你可以在你的应用程序使用多个RabbitMQ服务器,但将其中一个做为事件总线.

你可以使用任何[ConnectionFactry](http://rabbitmq.github.io/rabbitmq-dotnet-client/api/RabbitMQ.Client.ConnectionFactory.html#properties)属性作为连接属性.

**示例: 指定连接端口**

````csharp
{
  "RabbitMQ": {
    "Connections": {
      "Default": {
        "HostName": "123.123.123.123",
        "Port": "5672"
      }
    }
  }
}
````

如果需要连接到 RabbitMQ 集群，你可以指定多个 HostName。

**示例: 连接到 RabbitMQ 集群**

```json
{
  "RabbitMQ": {
    "Connections": {
      "Default": {
        "HostName": "123.123.123.123;234.234.234.234"
      }
    },
    "EventBus": {
      "ClientName": "MyClientName",
      "ExchangeName": "MyExchangeName"
    }
  }
}
```

### 选项类

`AbpRabbitMqOptions` 和 `AbpRabbitMqEventBusOptions` 类用于配置RabbitMQ的连接字符串和事件总线选项.

你可以在你的[模块](Module-Development-Basics.md)的 `ConfigureServices` 方法配置选项.

**示例: 配置连接**

````csharp
Configure<AbpRabbitMqOptions>(options =>
{
    options.Connections.Default.UserName = "user";
    options.Connections.Default.Password = "pass";
    options.Connections.Default.HostName = "123.123.123.123";
    options.Connections.Default.Port = 5672;
});
````

**示例: 配置客户端和交换机名称**

````csharp
Configure<AbpRabbitMqEventBusOptions>(options =>
{
    options.ClientName = "TestApp1";
    options.ExchangeName = "TestMessages";
});
````

使用这些选项类可以与 `appsettings.json` 组合在一起. 在代码中配置选项属性会覆盖配置文件中的值.
