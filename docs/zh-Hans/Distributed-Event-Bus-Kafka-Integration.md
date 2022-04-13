# 分布式事件总线Kafka集成

> 本文解释了 **如何配置[Kafka](https://kafka.apache.org/)** 做为分布式总线提供程序. 参阅[分布式事件总线文档](Distributed-Event-Bus.md)了解如何使用分布式事件总线系统.

## 安装

使用ABP CLI添加[Volo.Abp.EventBus.Kafka](https://www.nuget.org/packages/Volo.Abp.EventBus.Kafka)NuGet包到你的项目:

* 安装[ABP CLI](https://docs.abp.io/en/abp/latest/CLI),如果你还没有安装.
* 在你想要安装 `Volo.Abp.EventBus.Kafka` 包的 `.csproj` 文件目录打开命令行(终端).
* 运行 `abp add-package Volo.Abp.EventBus.Kafka` 命令.

如果你想要手动安装,安装[Volo.Abp.EventBus.Kafka](https://www.nuget.org/packages/Volo.Abp.EventBus.Kafka) NuGet 包到你的项目然后添加 `[DependsOn(typeof(AbpEventBusKafkaModule))]` 到你的项目[模块](Module-Development-Basics.md)类.

## 配置

可以使用配置使用标准的[配置系统](Configuration.md),如 `appsettings.json` 文件,或[选项](Options.md)类.

### `appsettings.json` 文件配置

这是配置Kafka设置最简单的方法. 它也非常强大,因为你可以使用[由AspNet Core支持](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)的任何其他配置源(如环境变量).

**示例：最小化配置与默认配置连接到本地的Kafka服务器**


````json
{
  "Kafka": {
    "EventBus": {
      "GroupId": "MyGroupId",
      "TopicName": "MyTopicName"
    }
  }
}
````

* `MyGroupId` 是应用程序的名称,用于Kafka的**GroupId**.
* `MyTopicName` 是**topic名称**.

参阅[Kafka文档](https://docs.confluent.io/current/clients/confluent-kafka-dotnet/api/Confluent.Kafka.html)更好的了解这些选项.

#### 连接

如果需要连接到本地主机以外的另一台服务器,需要配置连接属性.

**示例: 指定主机名 (如IP地址)**

````json
{
  "Kafka": {
    "Connections": {
      "Default": {
        "BootstrapServers": "123.123.123.123:9092"
      }
    },
    "EventBus": {
      "GroupId": "MyGroupId",
      "TopicName": "MyTopicName"
    }
  }
}
````

允许定义多个连接. 在这种情况下,你可以指定用于事件总线的连接.

**示例: 声明两个连接并将其中一个用于事件总线**

````json
{
  "Kafka": {
    "Connections": {
      "Default": {
        "BootstrapServers": "123.123.123.123:9092"
      },
      "SecondConnection": {
        "BootstrapServers": "321.321.321.321:9092"
      }
    },
    "EventBus": {
      "GroupId": "MyGroupId",
      "TopicName": "MyTopicName",
      "ConnectionName": "SecondConnection"
    }
  }
}
````

这允许你可以在你的应用程序使用多个Kafka服务器,但将其中一个做为事件总线.

你可以使用任何[ClientConfig](https://docs.confluent.io/current/clients/confluent-kafka-dotnet/api/Confluent.Kafka.ClientConfig.html)属性作为连接属性.

**示例: 指定socket超时时间**

````json
{
  "Kafka": {
    "Connections": {
      "Default": {
        "BootstrapServers": "123.123.123.123:9092",
        "SocketTimeoutMs": 60000
      }
    }
  }
}
````

### 选项类

`AbpKafkaOptions` 和 `AbpKafkaEventBusOptions` 类用于配置Kafka的连接字符串和事件总线选项.

你可以在你的[模块](Module-Development-Basics.md)的 `ConfigureServices` 方法配置选项.

**示例: 配置连接**

````csharp
Configure<AbpKafkaOptions>(options =>
{
    options.Connections.Default.BootstrapServers = "123.123.123.123:9092";
    options.Connections.Default.SaslUsername = "user";
    options.Connections.Default.SaslPassword = "pwd";
});
````

**示例: 配置 consumer config**

````csharp
Configure<AbpKafkaOptions>(options =>
{
    options.ConfigureConsumer = config =>
    {
        config.GroupId = "MyGroupId";
        config.EnableAutoCommit = false;
    };
});
````

**示例: 配置 producer config**

````csharp
Configure<AbpKafkaOptions>(options =>
{
    options.ConfigureProducer = config =>
    {
        config.MessageTimeoutMs = 6000;
        config.Acks = Acks.All;
    };
});
````

**示例: 配置 topic specification**

````csharp
Configure<AbpKafkaOptions>(options =>
{
    options.ConfigureTopic = specification =>
    {
        specification.ReplicationFactor = 3;
        specification.NumPartitions = 3;
    };
});
````

使用这些选项类可以与 `appsettings.json` 组合在一起. 在代码中配置选项属性会覆盖配置文件中的值.
