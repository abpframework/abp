# Distributed Event Bus Kafka Integration

> This document explains **how to configure the [Kafka](https://kafka.apache.org/)** as the distributed event bus provider. See the [distributed event bus document](Distributed-Event-Bus.md) to learn how to use the distributed event bus system

## Installation

Use the ABP CLI to add [Volo.Abp.EventBus.Kafka](https://www.nuget.org/packages/Volo.Abp.EventBus.Kafka) NuGet package to your project:

* Install the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) if you haven't installed before.
* Open a command line (terminal) in the directory of the `.csproj` file you want to add the `Volo.Abp.EventBus.Kafka` package.
* Run `abp add-package Volo.Abp.EventBus.Kafka` command.

If you want to do it manually, install the [Volo.Abp.EventBus.Kafka](https://www.nuget.org/packages/Volo.Abp.EventBus.Kafka) NuGet package to your project and add `[DependsOn(typeof(AbpEventBusKafkaModule))]` to the [ABP module](Module-Development-Basics.md) class inside your project.

## Configuration

You can configure using the standard [configuration system](Configuration.md), like using the `appsettings.json` file, or using the [options](Options.md) classes.

### `appsettings.json` file configuration

This is the simplest way to configure the Kafka settings. It is also very strong since you can use any other configuration source (like environment variables) that is [supported by the AspNet Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/).

**Example: The minimal configuration to connect to a local kafka server with default configurations**

````json
{
  "Kafka": {
    "Connections": {
      "Default": {
        "BootstrapServers": "localhost:9092"
      }
    },
    "EventBus": {
      "GroupId": "MyGroupId",
      "TopicName": "MyTopicName"
    }
  }
}
````

* `MyGroupId` is the name of this application, which is used as the **GroupId** on the Kakfa.
* `MyTopicName` is the **topic name**.

See [the Kafka document](https://docs.confluent.io/current/clients/confluent-kafka-dotnet/api/Confluent.Kafka.html) to understand these options better.

#### Connections

If you need to connect to another server than the localhost, you need to configure the connection properties.

**Example: Specify the host name (as an IP address)**

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

Defining multiple connections is allowed. In this case, you can specify the connection that is used for the event bus.

**Example: Declare two connections and use one of them for the event bus**

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

This allows you to use multiple Kafka cluster in your application, but select one of them for the event bus.

You can use any of the [ClientConfig](https://docs.confluent.io/current/clients/confluent-kafka-dotnet/api/Confluent.Kafka.ClientConfig.html) properties as the connection properties.

**Example: Specify the socket timeout**

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

### The Options Classes

`AbpRabbitMqOptions` and `AbpRabbitMqEventBusOptions` classes can be used to configure the connection strings and event bus options for the RabbitMQ.

You can configure this options inside the `ConfigureServices` of your [module](Module-Development-Basics.md).

**Example: Configure the connection**

````csharp
Configure<AbpKafkaOptions>(options =>
{
    options.Connections.Default.BootstrapServers = "123.123.123.123:9092";
    options.Connections.Default.SaslUsername = "user";
    options.Connections.Default.SaslPassword = "pwd";
});
````

**Example: Configure the consumer config**

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

**Example: Configure the producer config**

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

**Example: Configure the topic specification**

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

Using these options classes can be combined with the `appsettings.json` way. Configuring an option property in the code overrides the value in the configuration file.
