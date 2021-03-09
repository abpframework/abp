# RabbitMQ 后台作业管理

RabbitMQ 是一个标准的消息队列中间件,虽然它常用于消息传递/分布式事件,但也非常适合存储 FIFO(先进先出) 顺序的后台作业.

ABP Framework 提供了 [Volo.Abp.BackgroundJobs.RabbitMQ](https://www.nuget.org/packages/Volo.Abp.BackgroundJobs.RabbitMQ) 包,将使用 RabbitMQ 来执行后台作业.

> 参阅 [后台作业文档](Background-Jobs.md) 学习如何使用后台作业系统,本文只介绍了如何安装和配置 RabbitMQ 集成.

## 安装

使用 ABP CLI 将 [Volo.Abp.BackgroundJobs.RabbitMQ](https://www.nuget.org/packages/Volo.Abp.BackgroundJobs.RabbitMQ) 包添加到你的项目:

- 如果之前没有安装过 [ABP CLI](https://docs.abp.io/en/abp/latest/CLI),请先安装它.
- 跳转到待安装后台作业管理的项目目录中(包含 `.csproj` 文件的目录),打开终端管理器.
- 执行 `abp add-package Volo.Abp.BackgroundJobs.RabbitMQ` 命令.

如果你想要手动安装,请先用 NuGet 包管理器安装 [Volo.Abp.BackgroundJobs.RabbitMQ](https://www.nuget.org/packages/Volo.Abp.BackgroundJobs.RabbitMQ) 包到指定项目,之后使在你的 [模块](Module-Development-Basics.md) 上面添加 `[DependsOn(typeof(AbpBackgroundJobsRabbitMqModule))]` 配置依赖. 

## 配置

### 默认配置

默认配置将会使用标准端口和主机名(localhost)连接到 RabbitMQ 服务,**你不需要进行额外配置**.

### RabbitMQ 连接

你可以使用 ASP.NET Core 的 [标准配置系统](Configuration.md) 对 RabbitMQ 进行详细配置,比如 `appsettings.json` 或者是 [选项类](Options.md).

#### 通过  `appsettings.json` 文件配置

这种方式是配置 RabbitMQ 连接最简单的方式,你可以使用其他的配置源(例如环境变量).这些强大的功能都是由 [ASP.NET Core](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/configuration/) 提供的支持.

**示例: 配置默认的 RabbitMQ 连接**

```json
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
```

你可以在配置文件使用所有 [ConnectionFactry](http://rabbitmq.github.io/rabbitmq-dotnet-client/api/RabbitMQ.Client.ConnectionFactory.html#properties) 的属性,关于这些属性的具体含义,可以查看 RabbitMQ 的 [官方文档](https://www.rabbitmq.com/dotnet-api-guide.html#exchanges-and-queues).

目前我们允许定义多个连接,多连接的情况适用于不同的后台作业,具体配置信息可以参考下面的 RabbitMQ 后台作业配置说明.

**示例: 定义两个 RabbitMQ 连接**

```json
{
  "RabbitMQ": {
    "Connections": {
      "Default": {
        "HostName": "123.123.123.123"
      },
      "SecondConnection": {
        "HostName": "321.321.321.321"
      }
    }
  }
}
```

如果需要连接到 RabbitMQ 集群,你可以指定多个 HostName.

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

#### 使用选项类

`AbpRabbitMqOptions` 类型用于配置 RabbitMQ 的连接字符串,你可以在 [模块](Module-Development-Basics.md) 的 `ConfigureService` 方法中进行配置.

**示例: 配置 RabbitMQ 连接**

```csharp
Configure<AbpRabbitMqOptions>(options =>
{
    options.Connections.Default.UserName = "user";
    options.Connections.Default.Password = "pass";
    options.Connections.Default.HostName = "123.123.123.123";
    options.Connections.Default.Port = 5672;
});
```

关于选项类,可以结合 `appsettings.json` 文件一起使用.针对同一个属性,在选项类里面对该值进行了设定,会覆盖掉 `appsettings.json` 的值.

### RabbitMQ 后台作业配置说明

#### 后台作业队列的名称

默认情况下,每个后台作业都会使用一个单独的队列,结合标准前缀和作业名称来构造一个完整的队列名称.默认的前缀为 `AbpBackgroundJobs`,所以有一个作业的名称是 `EmailSending` 的话,在 RabbitMQ 的队列名称就是 `AbpBackgroundJobs.EmailSending`.

> 在后台作业的参数类上,可以使用 `BackgroundJobName` 特性指定后台作业的名称.否则的话,后台作业的名称将会是后台作业类的全名(也包含命名空间).

#### 后台作业使用的连接

默认情况下,后台作业都会使用 `Default` 作为默认连接.

#### 自定义

`AbpRabbitMqBackgroundJobOptions` 可以自定义队列名和作业使用的 RabbitMQ 连接.

**示例: **

```csharp
Configure<AbpRabbitMqBackgroundJobOptions>(options =>
{
    options.DefaultQueueNamePrefix = "my_app_jobs.";
    options.JobQueues[typeof(EmailSendingArgs)] =
        new JobQueueConfiguration(
            typeof(EmailSendingArgs),
            queueName: "my_app_jobs.emails",
            connectionName: "SecondConnection"
        );
});
```

- 这个示例将默认的队列名前缀设置为 `my_app_jobs.`,如果多个项目都使用的同一个 RabbitMQ 服务,设置不同的前缀可以避免执行其他项目的后台作业.
- 这里还设置了 `EmailSendingArgs` 绑定的 RabbitMQ 连接.

`JobQueueConfiguration` 类的构造函数中,还有一些其他的可选参数.

- `queueName`: 指定后台作业对应的队列名称(全名).
- `connectionName`: 后台作业对应的 RabbitMQ 连接名称,默认是 `Default`.
- `durable`: 可选参数,默认为 `true`.
- `exclusive`: 可选参数,默认为 `false`.
- `autoDelete`: 可选参数,默认为 `false`.

如果你想要更多地了解 `durable`,`exclusive`,`autoDelete` 的用法,请阅读 RabbitMQ 提供的文档.

## 另请参阅

- [后台作业](Background-Jobs.md)
